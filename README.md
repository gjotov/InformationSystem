# Информационная система по Физике (C#)

Данная информационная система разработана на языке программирования C# для помощи в изучении физики и решении задач. Система предоставляет возможность использования специальных калькуляторов и просмотра теории.

### [Скриншоты программы](https://drive.google.com/drive/folders/1n94XEREqiIbxn08Pmf06TyqMiawfNXzV?usp=sharing)
*Хранение изображений: Google.disk*

![Фото проекта](https://i.postimg.cc/3wKnrPGG/Screenshot-1.png)

## Функциональность

1. **Навигация**: Пользователь имеет возможность удобного перемещения между темами по вкладкам.

2. **Калькулятор з-на Ома**: Пользователь может использовать специальный калькулятор для рассчета Напряжения, Сопративления и Силы тока по закону Ома.

3. **Конвертатор значений**: В системе пользователь может с легкостью переводить значения в степени.

4. **Содержание**: Система выводит из базы данных содежание тем, которые можно будет просматривать.

5. **Дополнительное**: На интерфейсе присутствуют небольшие полезные подсказки по темам.

## Технические детали

- **Язык программирования**: C#
- **База данных**: SQLite (с использованием библиотеки Microsoft.Data.Sqlite)
- **Калькуляторы**

  ## Пример работы с базой данных SQLite
  Для работы с базой данных SQLite в данном примере используется библиотека Microsoft.Data.Sqlite в среде программирования C#. Пример демонстрирует поиск клиента в базе данных по названию темы.

  ``` C#
  public class Database
{
    private string connectionString;

    public Database(string dbPath)
    {
        connectionString = $"Data Source={dbPath};Version=3;New=False;Compress=True;";
    }
    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }

    public DataTable GetTopicContent(string topicName)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM ThermalMotion WHERE name = @topicName";
                cmd.Parameters.AddWithValue("@topicName", topicName);

                using (var reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }
    }
}
```

