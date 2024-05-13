using Microsoft.EntityFrameworkCore;

namespace Dz13._05._024.Models {
    public class Context : DbContext {
        public DbSet<Films> Films { get; set; }
        public Context(DbContextOptions<Context> options)
           : base(options) {
            if (Database.EnsureCreated()) {
                Films!.AddRange(new List<Films> {
                    new Films {
                        Title = "Джон Уик 4",
                        Director = "Чад Стахелски",
                        Genre = "Боевик",
                        Date = new DateTime(2023, 3, 24),
                        PosterPath = "/Images/1.jpg",
                        Description = "Джон Уик находит способ одержать победу над Правлением Кланов. Однако, прежде чем он сможет заслужить свою свободу, ему предстоит сразиться с новым врагом и его могущественными союзниками."
                    },
                    new Films {
                        Title = "Драйв",
                        Director = "Николас Виндинг Рефн",
                        Genre = "Боевик",
                        Date = new DateTime(2011, 9, 16),
                        PosterPath = "/Images/2.jpg",
                        Description = "Великолепный гонщик — при свете дня он выполняет каскадерские трюки на съёмочных площадках Голливуда, а по ночам ведет рискованную игру."
                    },
                    new Films {
                        Title = "Мандалорец",
                        Director = "Джон фавро",
                        Genre = "Приключения",
                        Date = new DateTime(2019, 11, 12),
                        PosterPath = "/Images/3.jpg",
                        Description = "Это первый игровой сериал, являющийся составной частью вселенной «Звёздных войн». Действие «Мандалорца» разворачивается пять лет спустя после событий фильма «Возвращение джедая»."
                    },
                    new Films {
                        Title = "Бойцовский клуб",
                        Director = "Дэвид Финчер",
                        Genre = "Триллер",
                        Date = new DateTime(1999, 10, 15),
                        PosterPath = "/Images/4.jpg",
                        Description = "Терзаемый хронической бессонницей и отчаянно пытающийся вырваться из мучительно скучной жизни клерк встречает некоего Тайлера Дардена, харизматического торговца мылом с извращенной философией."
                    },
                    new Films {
                        Title = "Шерлок",
                        Director = "Марк Гэттис",
                        Genre = "Криминальная драмма",
                        Date = new DateTime(2010, 7, 25),
                        PosterPath = "/Images/5.jpg",
                        Description = "Шерлок Холмс — британский сериал в жанре детективной криминальной драмы, основанный на произведениях сэра Артура Конан Дойля о детективе Шерлоке Холмсе."
                    },
                    new Films {
                        Title = "Никто",
                        Director = "Илья Найшуллер",
                        Genre = "Боевик",
                        Date = new DateTime(2021, 3, 26),
                        PosterPath = "/Images/6.jpg",
                        Description = "Непримечательный и незаметный человек живёт обычной жизнью, пока однажды, спасая женщину от нападения бандитов, не отправляет одного из хулиганов в больницу."
                    }
                });
                SaveChanges();
            }
        }
    }
}