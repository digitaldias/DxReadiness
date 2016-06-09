using DxReadinessSolution.Business;
using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Entities;
using DxReadinessSolution.Fakes;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string subscriptionKeyEmotion  = ImageAnalyzerConfiguration.SubscriptionKeyEmotion;
        private string subscriptionKeyVision   = ImageAnalyzerConfiguration.SubscriptionKeyVision;
        private string connectionString = ImageAnalyzerConfiguration.ConnectionStringEventHub;

        private readonly ExceptionHandler _exceptionHandler;

        public ImageAnalyzer()
        {
            var logger = new Logger();
            _exceptionHandler = new ExceptionHandler(logger);
        }

        public async Task<ImageResult> AnalyzeImage(Stream imageStream)
        {
            VisionServiceClient visionServiceClient   = new VisionServiceClient(subscriptionKeyVision);
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(subscriptionKeyEmotion);
            
            MemoryStream stream2 = _exceptionHandler.Get(() => CreateStreamCopy(imageStream));

            var visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
            var emotionResult = await _exceptionHandler.Get(() => emotionServiceClient.RecognizeAsync(imageStream));
            var analysisResult = await _exceptionHandler.Get(() => visionServiceClient.AnalyzeImageAsync(stream2, visualFeatures));

            var imageResult = CreateImageResultFromAnalysis(analysisResult, emotionResult);

            PostImageResultToEventHub(imageResult);

            return imageResult;
        }


        private static MemoryStream CreateStreamCopy(Stream imageStream)
        {
            MemoryStream stream2 = new MemoryStream();
            imageStream.CopyTo(stream2);
            imageStream.Seek(0, SeekOrigin.Begin);
            stream2.Seek(0, SeekOrigin.Begin);

            return stream2;
        }


        private void PostImageResultToEventHub(ImageResult result)
        {      
            var json            = JsonConvert.SerializeObject(result);
            var bytes           = Encoding.UTF8.GetBytes(json);
            EventData sendEvent = new EventData(bytes);

            _exceptionHandler.Run(() => 
            {
                EventHubClient ehClient = EventHubClient.CreateFromConnectionString(connectionString, "picturificeventhub");
                ehClient.SendAsync(sendEvent);
            });
        }


        private ImageResult CreateImageResultFromAnalysis(AnalysisResult analysisResult, Emotion[] emotionResult)
        {
            ImageResult imageResult = new ImageResult();

            imageResult.Categories.AddRange(analysisResult.Categories.Where(category => category.Score > 0.6).Select(category => category.Name));
            imageResult.Tags.AddRange(analysisResult.Tags.Where(tag => tag.Confidence > 0.6).Select(tag => tag.Name));

            imageResult.MenFaces += analysisResult.Faces.Where(face => face.Gender == "Male").Count();
            imageResult.WomenFaces += analysisResult.Faces.Where(face => face.Gender == "Female").Count();

            CreateEmotionDictionariesAndAddThemToResult(emotionResult, imageResult);

            return imageResult;
        }

        private static void CreateEmotionDictionariesAndAddThemToResult(Emotion[] emotionResult, ImageResult result)
        {
            foreach (var emotion in emotionResult)
            {
                var emotionDictionary = new Dictionary<string, float>();
                AddEmotionValuesToDictionary(emotion, emotionDictionary);
                result.Emotions.Add(emotionDictionary);
            }
        }

        private static void AddEmotionValuesToDictionary(Emotion emotion, Dictionary<string, float> emotionDictionary)
        {
            const double THRESHOLD_FOR_ADDING = 0.6 ;

            var emotionType  = emotion.Scores.GetType();
            var propertyList = emotionType
                .GetProperties()
                .Select(property => new { Name = property.Name, Value = (float)property.GetValue(emotion.Scores) })
                .Where(o => o.Value > THRESHOLD_FOR_ADDING)
                .OrderByDescending(o => o.Value)
                .ToList();

            foreach (var property in propertyList)
                emotionDictionary.Add(property.Name, property.Value);
        }
    }
}
