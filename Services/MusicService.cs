using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SongsInLearning.Database;
using SongsInLearning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongsInLearning.Services;

public class MusicService
{
    private readonly MusicDbContext _context;

    public MusicService(MusicDbContext context)
    {
        _context = context;
    }

    public Task<List<Music>> GetAllAsync()
    {
       return _context.Musics.ToListAsync();
    }

    public async Task AddAsync(Music music)
    {
        IAService iaService = new();

        var iaInfos = await iaService.CreateAnnotationAsync(music);

        string iaInfosToString = Convert.ToString(iaInfos)!;


        if (iaInfosToString.IsNullOrEmpty())
        {
            iaInfosToString = "Failed to generate IA Infos";
        }

        music.InfosGeneratedByIA = iaInfosToString;

        _context.Musics.Add(music);
        await _context.SaveChangesAsync();
    }
}
