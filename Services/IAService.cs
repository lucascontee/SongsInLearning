using Microsoft.Identity.Client;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using SongsInLearning.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SongsInLearning.Services;
public class IAService
{
    GoogleAI googleAI = new GoogleAI(apiKey: "AIzaSyA961VesPlJwOuVWJxJoEqNqvbADaER3WE");

    RegionInfo currentRegion = RegionInfo.CurrentRegion;

    public async Task<string> CreateAnnotationAsync(Music music)
    {
        string countryName = currentRegion.EnglishName;

        var systemInstruction = new Content("""
            
            You are a professional music trivia researcher. Your primary goal is to find interesting facts, anecdotes, or historical details about the song you receive.

            You will be given the following information to facilitate your search:
            - **Song Title:** [Title of the song]
            - **Artist:** [Name of the artist/band]
            - **Year:** [Year the song was released/recorded]
            - **Instrument:** [Specific instrument to research (e.g., "guitar", "bass", "drums", "vocals")]
            - **User Country:** [Country of the user, e.g., "Brazil", "USA", "Canada"]

            Here's what you need to do:
            1.  **Research the song's creation:** Find stories and trivia about the recording process, the songwriting, or how the song came to be.
            2.  **Focus on the specified instrument:** Research who played the given instrument on that song. If possible, find information about the specific equipment (e.g., guitar model, amplifier, specific drum kit, microphone type) they used for that recording.
            3.  **Utilize Google Search for your research.**
            4.  **Language Adaptation:** Based on the 'User Country', provide the response in the primary language spoken in that country. If the country has multiple primary languages, use the most commonly spoken one.
            5.  **Output Length:** Generate a response that is concise and **does not exceed 2500 characters.**

            **Return only the research text, without any introductory phrases like "Here's a search for..." or "Okay, here's some information about the.." or similar.** The output will be displayed directly on a front-end.          
            """);

        var model = googleAI.GenerativeModel(model: Model.Gemini20Flash, systemInstruction: systemInstruction);
        model.UseGoogleSearch = true;

        string prompt = $"""
            Song Title: {music.Name}
            Artist: {music.Artist}
            Year: {music.Year}
            Instrument: {music.Instrument}
            User Country: {countryName}
            """;

        var request = new GenerateContentRequest(prompt);

        GenerateContentResponse response = await model.GenerateContent(request);
        string responseToString = CleanResponseString(response);

        return responseToString;
    }

    public string CleanResponseString(GenerateContentResponse responseContent)
    {

        string responseToString = Convert.ToString(responseContent)!;

        if (string.IsNullOrEmpty(responseToString))
        {
            return string.Empty;
        }

        
        string cleanedText = Regex.Replace(responseToString, @"[^\p{L}\p{N}\s.,!?;:\-()/'""]", string.Empty);

        cleanedText = Regex.Replace(cleanedText, @"\s+", " ").Trim(); 
        cleanedText = Regex.Replace(cleanedText, @"(\r\n|\r|\n){2,}", "\r\n\r\n");

        return cleanedText;

    }
}
