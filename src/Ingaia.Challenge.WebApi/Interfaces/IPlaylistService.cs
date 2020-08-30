using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ingaia.Challenge.WebApi.Interfaces
{
    public interface IPlaylistService
    {
        Task<IEnumerable<string>> GetPlaylistTracks(string genre);
    }
}
