using Microsoft.Extensions.Configuration;
using MusicPortal.Models;

namespace MusicPortal.BLL.Services {
    public interface ILangService {
        List<Language> languageList();
    }
    public class LangService : ILangService {
        IConfiguration config;
        List<Language> langList;
        public LangService(IConfiguration cfg) {
            config = cfg;
            IConfigurationSection pointSection = config.GetSection("Lang");
            List<Language> lists = new List<Language>();
            foreach (var language in pointSection.AsEnumerable()) {
                if (language.Value != null) lists.Add(new Language {
                    ShortName = language.Key.Replace("Lang" + ":", ""),
                    Name = language.Value
                });
            }
            langList = lists;
        }
        public List<Language> languageList() => langList;
    }
}
