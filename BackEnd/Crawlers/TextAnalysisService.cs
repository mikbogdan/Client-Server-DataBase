using Pullenti.Ner;
using Pullenti.Ner.Geo;
using Pullenti.Ner.Org;
using Pullenti.Ner.Person;
using static System.Net.Mime.MediaTypeNames;

namespace NewsParser.TextServices
{
    public class TextAnalysisService
    {
        public List<Entity> ExtractEntities(string content)
        {
            // Инициализация процессора Pullenti
            Pullenti.Sdk.InitializeAll();

            Processor processor = ProcessorService.CreateProcessor();
            var geoAnalyzer = new GeoAnalyzer();
            var personAnalyzer = new PersonAnalyzer();
            var orgAnalyzer = new OrganizationAnalyzer();

            processor.AddAnalyzer(geoAnalyzer);
            processor.AddAnalyzer(personAnalyzer);
            processor.AddAnalyzer(orgAnalyzer);

            // Анализ текста
            var result = processor.Process(new SourceOfAnalysis(content));
            var entities = new List<Entity>();

            // Получаем все сущности через GetReferents()
            foreach (var referent in result.Entities)
            {
                if (referent == null) continue;

                // Определяем тип сущности
                string type = referent switch
                {
                    GeoReferent => "Location",
                    OrganizationReferent => "Organization",
                    PersonReferent => "Person",
                    _ => "Unknown"
                };

                if (type != "Unknown")
                {
                    var entityInfo = new Entity
                    {
                        Value = referent.ToString(),
                    };
                    entities.Add(entityInfo);
                }
            }
            return entities;
        }
    }

    public class Entity
    {
        public required string Value { get; set; } // Значение сущности
    }
}