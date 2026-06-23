using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SongsInLearning.Database;
using SongsInLearning.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SongsInLearning.Services;

public class SongService
{
    private readonly MusicDbContext _context;
    private readonly IAService _iAService;

    public SongService(MusicDbContext context, IAService iaService)
    {
        _context = context;
        _iAService = iaService;
    }

    public Task<List<Song>> GetAllAsync()
    {
       return _context.Musics.ToListAsync();
    }

    public async Task AddAsync(Song song)
    {

        string iaInfosToString = "Failed to generate IA Infos";


        song.InfosGeneratedByIA = iaInfosToString;

        _context.Musics.Add(song);
        await _context.SaveChangesAsync();
    }
}
