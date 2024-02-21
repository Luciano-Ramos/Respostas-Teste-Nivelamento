using Newtonsoft.Json;

public class Program
{
    private static async Task Main(string[] args)
    {
        await GetFootballMatchesAsync(2013, "Paris Saint-Germain");
    }


    private static async Task GetFootballMatchesAsync(int year, string team)
    {
        using (HttpClient client = new HttpClient())
        {
            int currentPage = 1;
            int totalPages = int.MaxValue;
            List<FootballMatch> allMatches = new List<FootballMatch>();

            while (currentPage <= totalPages)
            {


                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    FootballMatchesResponse matchesResponse = JsonConvert.DeserializeObject<FootballMatchesResponse>(jsonResponse);


                    if (currentPage == 1)
                    {
                        totalPages = matchesResponse.total_pages;
                        Console.WriteLine($"Total pages: {totalPages}");
                    }

                    allMatches.AddRange(matchesResponse.Data);

                    currentPage++;
                }
                else
                {
                    Console.WriteLine($"Erro ao fazer solicitação: {response.StatusCode}");
                    break;
                }
            }


            Dictionary<string, int> goalsByTeam = new Dictionary<string, int>();

            foreach (var match in allMatches)
            {
                if (!string.IsNullOrEmpty(match.team1))
                {
                    if (!goalsByTeam.ContainsKey(match.team1))
                        goalsByTeam[match.team1] = 0;

                    goalsByTeam[match.team1] += match.team1goals;
                }

                if (!string.IsNullOrEmpty(match.team2))
                {
                    if (!goalsByTeam.ContainsKey(match.team2))
                        goalsByTeam[match.team2] = 0;

                    goalsByTeam[match.team2] += match.team2goals;
                }
            }


            foreach (var kvp in goalsByTeam)
            {
                Console.WriteLine("Team " + kvp.Key + " scored " + kvp.Value + " goals in " + year);
            }

        }
    }


    private class FootballMatchesResponse
    {
        public int total_pages { get; set; }
        public FootballMatch[] Data { get; set; }
    }

    private class FootballMatch
    {
        public int year { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public int team1goals { get; set; }
        public int team2goals { get; set; }
        public int page { get; set; }
    }

}