using DxReadinessSolution.Domain.Contracts;
using DxReadinessSolution.Domain.Entities;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string subscriptionKeyEmotion = ImageAnalyzerConfiguration.SubscriptionKeyEmotion;
        private string subscriptionKeyVision = ImageAnalyzerConfiguration.SubscriptionKeyVision;
        
        
        public async Task<AnalysisResult> AnalyzeImage(Stream imageStream)
        {
            VisionServiceClient VisionServiceClient = new VisionServiceClient(subscriptionKeyVision);
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(subscriptionKeyEmotion);
            
            using (imageStream )
            {
                VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                AnalysisResult analysisResult  = await VisionServiceClient.AnalyzeImageAsync(imageStream, visualFeatures);
                Emotion[] emotionResult = await emotionServiceClient.RecognizeAsync(imageStream);
                return analysisResult;
            }
        }


     
    }
}
