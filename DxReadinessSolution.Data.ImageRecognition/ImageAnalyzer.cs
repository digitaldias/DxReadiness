using DxReadinessSolution.Domain.Contracts;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DxReadinessSolution.Data.ImageRecognition
{
    public class ImageAnalyzer : IImageAnalyzer
    {
        private string SubscriptionKey = "2aa91152614543edb036c0bd8f24c092";
        private string _status;


        /// <summary>
        /// Uploads the image to Project Oxford and performs analysis
        /// </summary>
        /// <param name="imageFilePath">The image file path.</param>
        /// <returns></returns>
        public async Task<AnalysisResult> AnalyzeImage(Stream imageStream)
        {
            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE STARTS HERE
            // -----------------------------------------------------------------------

            //
            // Create Project Oxford Vision API Service client
            //
            VisionServiceClient VisionServiceClient = new VisionServiceClient(SubscriptionKey);
            Console.WriteLine("VisionServiceClient is created");

            using (imageStream )
            {
                //
                // Analyze the image for all visual features
                //
                Console.WriteLine("Calling VisionServiceClient.AnalyzeImageAsync()...");
                VisualFeature[] visualFeatures = new VisualFeature[] { VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType, VisualFeature.Tags };
                AnalysisResult analysisResult = await VisionServiceClient.AnalyzeImageAsync(imageStream, visualFeatures);
                return analysisResult;
            }

            // -----------------------------------------------------------------------
            // KEY SAMPLE CODE ENDS HERE
            // -----------------------------------------------------------------------
        }



        /// <summary>
        /// Perform the work for this scenario
        /// </summary>
        /// <param name="imageUri">The URI of the image to run against the scenario</param>
        /// <param name="upload">Upload the image to Project Oxford if [true]; submit the Uri as a remote url if [false];</param>
        /// <returns></returns>
        protected async Task DoWork(Stream imageStream)
        {
            _status = "Analyzing...";

            //
            // Either upload an image, or supply a url
            //
            AnalysisResult analysisResult;
            
            analysisResult = await AnalyzeImage(imageStream);
            
            _status = "Analyzing Done";

            //
            // Log analysis result in the log window
            //
            Console.WriteLine("");
            Console.WriteLine("Analysis Result:");
            //LogAnalysisResult(analysisResult);
        }
    }
}
