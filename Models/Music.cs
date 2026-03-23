using SongsInLearning.Models.Enums;
using System;
using Instrument = SongsInLearning.Models.Enums.Instrument;

namespace SongsInLearning.Models;
public class Music
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public int Year { get; set; }
    public Difficulty Difficulty { get; set; } = Difficulty.Medium;
    public Tuning Tuning { get; set; } = Tuning.DefaultTuning;
    public Progress Progress { get; set; } = Progress.Learn; 
    public string InfosGeneratedByIA { get; set; } = string.Empty; 
    public string UserAnnotations { get; set; } = string.Empty;
    public Instrument Instrument { get; set; } = Instrument.None;
    public double Bpm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
