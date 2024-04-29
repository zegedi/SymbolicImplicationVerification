using Microsoft.VisualBasic;
using SymbolicImplicationVerification.Converts;
using SymbolicImplicationVerification.Model;

namespace SymbolicImplicationVerification.Persistence
{
    //public class ModelDataAccess : IDataAccess<SimImplyModel>
    //{
    //    private const char separator = ';';

    //    /// <summary>
    //    /// Loads a <see cref="SimImplyModel"/> object, from the given source file.
    //    /// </summary>
    //    /// <param name="path">The path of the source file.</param>
    //    /// <returns>The loaded <see cref="SimImplyModel"/> object.</returns>
    //    public async Task<SimImplyModel> LoadAsync(string path)
    //    {
    //        using (StreamReader reader = new StreamReader(path))
    //        {
    //            string line = await reader.ReadLineAsync() ?? String.Empty;
    //            String[] numbers = line.Split(' '); // beolvasunk egy sort, és a szóköz mentén széttöredezzük
    //            Int32 tableSize = Int32.Parse(numbers[0]); // beolvassuk a tábla méretét
    //            Int32 regionSize = Int32.Parse(numbers[1]); // beolvassuk a házak méretét
    //            SudokuTable table = new SudokuTable(tableSize, regionSize); // létrehozzuk a táblát

    //            for (Int32 i = 0; i < tableSize; i++)
    //            {
    //                line = await reader.ReadLineAsync() ?? String.Empty;
    //                numbers = line.Split(' ');

    //                for (Int32 j = 0; j < tableSize; j++)
    //                {
    //                    table.SetValue(i, j, Int32.Parse(numbers[j]), false);
    //                }
    //            }

    //            for (Int32 i = 0; i < tableSize; i++)
    //            {
    //                line = await reader.ReadLineAsync() ?? String.Empty;
    //                String[] locks = line.Split(' ');

    //                for (Int32 j = 0; j < tableSize; j++)
    //                {
    //                    if (locks[j] == "1")
    //                    {
    //                        table.SetLock(i, j);
    //                    }
    //                }
    //            }

    //            return table;
    //        }
    //    }

    //    /// <summary>
    //    /// Saves the given object.
    //    /// </summary>
    //    /// <param name="path">The path of the destination file.</param>
    //    /// <param name="obj">The object to save.</param>
    //    public async Task SaveAsync(string path, SimImplyModel model)
    //    {

    //    }
    //}
}
