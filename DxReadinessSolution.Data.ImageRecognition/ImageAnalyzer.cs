using DxReadinessSolution.Business;
using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Entities;
using DxReadinessSolution.Fakes;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string subscriptionKeyEmotion = ImageAnalyzerConfiguration.SubscriptionKeyEmotion;
        private string subscriptionKeyVision = ImageAnalyzerConfiguration.SubscriptionKeyVision;
        
        
        public async Task<ImageResult> AnalyzeImage(Stream imageStream)
        {
            VisionServiceClient visionServiceClient = new VisionServiceClient(subscriptionKeyVision);
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(subscriptionKeyEmotion);
            
            using (imageStream )
            {
                MemoryStream stream2 = new MemoryStream();
                imageStream.CopyTo(stream2);
                imageStream.Seek(0, SeekOrigin.Begin);
                stream2.Seek(0, SeekOrigin.Begin);

                var visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };

                var exceptionHandler = new ExceptionHandler(new Logger());
                var emotionResult = await exceptionHandler.Get(() => 
                    emotionServiceClient.RecognizeAsync (imageStream));                   

                var analysisResult = await exceptionHandler.Get(()=>
                     visionServiceClient.AnalyzeImageAsync(stream2, visualFeatures)
                    );

                var imageResult = createImageResult(analysisResult, emotionResult);

                return imageResult;
            }
            
        }

        private ImageResult createImageResult(AnalysisResult analysisResult, Emotion[] emotionResult)
        {
            ImageResult result = new ImageResult();

            foreach(var cat in analysisResult.Categories)
            {
                if (cat.Score > 0.6)
                    result.Categories.Add(cat.Name);
            }

            foreach (var face in analysisResult.Faces)
            {
                result.Ages.Add(face.Age);

                if (face.Gender == "Male")
                    result.MenFaces++;
                else
                    result.WomenFaces++;
            }

            foreach(var tag in analysisResult.Tags)
            {
                if (tag.Confidence > 0.6)
                    result.Tags.Add(tag.Name);
            }

            foreach (var emotion in emotionResult)
            {
                var em = new Dictionary<string, float>();
                AddEmotions(emotion, em);
                result.Emotions.Add(em);
            }

            return result;
        }

        private static void AddEmotions(Emotion emotion, Dictionary<string, float> em)
        {

            //var firstPerson = emotionResult[0];

            //var type = emotion.Scores.GetType();

            //var list = type
            //.GetProperties()
            //.Select(property => new
            //{
            //    Name = property.Name,
            //    Value = (float)property.GetValue(property)
            //}).ToList();

            //list.Where(o => o.Value > 0.7)
            //   .OrderByDescending(o => o.Value);

            const double threshold = 0.7 ;

            if (emotion.Scores.Anger > threshold)
            {
                em.Add("Anger", emotion.Scores.Anger);
            }
            if (emotion.Scores.Contempt > threshold)
            {
                em.Add("Contempt", emotion.Scores.Contempt);
            }

            if (emotion.Scores.Disgust > threshold)
            {
                em.Add("Disgust", emotion.Scores.Disgust);
            }

            if (emotion.Scores.Fear > threshold)
            {
                em.Add("Fear", emotion.Scores.Fear);
            }

            if (emotion.Scores.Happiness > threshold)
            {
                em.Add("Happiness", emotion.Scores.Happiness);
            }

            if (emotion.Scores.Neutral > threshold)
            {
                em.Add("Neutral", emotion.Scores.Neutral);
            }

            if (emotion.Scores.Sadness > threshold)
            {
                em.Add("Sadness", emotion.Scores.Sadness);
            }

            if (emotion.Scores.Surprise > threshold)
            {
                em.Add("Surprise", emotion.Scores.Surprise);
            }
        }
    }
}
