namespace Ingaia.Challenge.WebApi.Constants.Logs
{
    public static class PlaylistLogConstants
    {
        public const string GENRE_NOT_FOUND = "Não foi possível encontrar nenhuma playlist para o genêro música informado: {genre}.";
        public const string NOT_FOUND = "Não foi possível obter detalhes da playlist selecionada: {id}.";

        public const string STREAMING_SEARCH_ERROR = "Ocorreu um erro ao buscar a playlist no serviço de streaming. PlaylistId: {id}";
    }
}
