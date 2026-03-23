using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.Models;

public class ListItemTemplate
{
    public string Label { get; set; } = string.Empty;
    public Type? ModelType { get; set; }
    public string IconKey { get; set; } = string.Empty;

    public ListItemTemplate(string label, Type? modelType)
    {
        Label = label;
        ModelType = modelType;
    }
}
