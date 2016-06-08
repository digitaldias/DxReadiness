﻿using DxReadinessSolution.Domain.Contracts;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string SubscriptionKey = "2aa91152614543edb036c0bd8f24c092";
        
        
        public async Task<AnalysisResult> AnalyzeImage(Stream imageStream)
        {
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Console.WriteLine("VisionServiceClient is created");

            using (imageStream )
            {
                Console.WriteLine("Calling VisionServiceClient.AnalyzeImageAsync()...");

                VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                AnalysisResult analysisResult  = await VisionServiceClient.AnalyzeImageAsync(imageStream, visualFeatures);

                return analysisResult;
            }
        }


        protected async Task DoWork(Stream imageStream)
        {
            AnalysisResult analysisResult;
            
            analysisResult = await AnalyzeImage(imageStream);
            
            Console.WriteLine("");
            Console.WriteLine("Analysis Result:");
        }
    }
}
