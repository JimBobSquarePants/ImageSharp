﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageHelpers.cs" company="James South">
//   Copyright (c) James South.
//   Licensed under the Apache License, Version 2.0.
// </copyright>
// <summary>
//   The image helpers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ImageProcessor.Web.Helpers
{
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using ImageProcessor.Configuration;
    using ImageProcessor.Imaging.Formats;

    /// <summary>
    /// The image helpers.
    /// </summary>
    public static class ImageHelpers
    {
        /// <summary>
        /// The regex pattern.
        /// </summary>
        public static readonly string ExtensionRegexPattern = BuildExtensionRegexPattern();

        /// <summary>
        /// The exclude regex for matching things to ignore when parsing image extensions.
        /// I'd like to make something more extensible than this.
        /// </summary>
        private static readonly Regex ExcludeRegex = new Regex(@"mask=[\w+-]+.", RegexOptions.IgnoreCase);

        /// <summary>
        /// The image format regex.
        /// </summary>
        private static readonly Regex FormatRegex = new Regex(@"(\.?)(png8|" + ExtensionRegexPattern + ")", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

        /// <summary>
        /// The image format regex for matching the file format at the end of a string.
        /// </summary>
        private static readonly Regex EndFormatRegex = new Regex(@"(\.)" + ExtensionRegexPattern + "$", RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

        /// <summary>
        /// Checks a given string to check whether the value contains a valid image extension.
        /// </summary>
        /// <param name="fileName">The string containing the filename to check.</param>
        /// <returns>True the value contains a valid image extension, otherwise false.</returns>
        public static bool IsValidImageExtension(string fileName)
        {
            return EndFormatRegex.IsMatch(fileName);
        }

        /// <summary>
        /// Returns the correct file extension for the given string input
        /// </summary>
        /// <param name="input">
        /// The string to parse.
        /// </param>
        /// <returns>
        /// The correct file extension for the given string input if it can find one; otherwise an empty string.
        /// </returns>
        public static string GetExtension(string input)
        {
            // First filter out any troublesome elements.
            foreach (Match exlude in ExcludeRegex.Matches(input))
            {
                input = input.Replace(exlude.Value, string.Empty);
            }

            Match match = FormatRegex.Match(input);

            if (match.Success)
            {
                // Ah the enigma that is the png file.
                if (match.Value.ToLowerInvariant().EndsWith("png8"))
                {
                    return "png";
                }

                return match.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the correct mime-type for the given string input.
        /// </summary>
        /// <param name="path">
        /// The path to the cached image.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> matching the correct mime-type.
        /// </returns>
        public static string GetMimeType(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, false))
            {
                ISupportedImageFormat format = FormatUtilities.GetFormat(file);

                if (format != null)
                {
                    return format.MimeType;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Builds a regular expression from the <see cref="T:ImageProcessor.Imaging.Formats.ISupportedImageFormat"/> type, this allows extensibility.
        /// </summary>
        /// <returns>
        /// The <see cref="Regex"/> to match matrix filters.
        /// </returns>
        private static string BuildExtensionRegexPattern()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("(");
            int counter = 0;
            foreach (ISupportedImageFormat imageFormat in ImageProcessorBootstrapper.Instance.SupportedImageFormats)
            {
                foreach (string fileExtension in imageFormat.FileExtensions)
                {
                    if (counter == 0)
                    {
                        stringBuilder.Append(fileExtension.ToLowerInvariant());
                    }
                    else
                    {
                        stringBuilder.AppendFormat("|{0}", fileExtension.ToLowerInvariant());
                    }
                }

                counter++;
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}