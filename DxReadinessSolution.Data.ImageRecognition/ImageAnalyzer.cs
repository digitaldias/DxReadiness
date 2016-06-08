using DxReadinessSolution.Business;
using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Entities;
using DxReadinessSolution.Fakes;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string subscriptionKeyEmotion = ImageAnalyzerConfiguration.SubscriptionKeyEmotion;
        private string subscriptionKeyVision = ImageAnalyzerConfiguration.SubscriptionKeyVision;
        
        
        public async Task<AnalysisResult> AnalyzeImage(Stream imageStream)
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

                GetHighestEmotion(emotionResult);

                return analysisResult;
            }
            
        }

        private void GetHighestEmotion(Emotion[] emotionResult)
        {
            var firstPerson = emotionResult[0];

            var type = firstPerson.Scores.GetType();

            var list = type
                .GetProperties()
                .Select(property => new
                {
                    Name = property.Name,
                    Value = (float)property.GetValue(property)
                });

            list.OrderByDescending(o => o.Value);

        }
    }
}
