using Ingaia.Challenge.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface IPlaylistService
    {
        Task<IEnumerable<string>> GetPlaylistTracksAsync(string genre);
    }
}
