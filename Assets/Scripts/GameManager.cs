using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject dayPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private BottomPages pages;

    [SerializeField] private GameObject infoTxt;

    public static GameManager inst;

    public static string Path;

    private void Awake()
    {
        inst = this;

        Path = Application.persistentDataPath + "/file.xlsx";

        // 2 PIN, 83 Language
        Loader.FileLoaded += FileLoaded;
        Loader.GetFile(83);
    }

    private XSSFWorkbook wk;
    private FileStream fs;

    private ISheet sheet;           // Рабочий лист
    private IRow row;               //Строка
    private ICell cell;             // Столбец

    private void FileLoaded(bool val)
    {
        Loader.FileLoaded -= FileLoaded;

        if (val)
            StartCoroutine(InfoCor());

        if (!File.Exists(Path)) return;

        fs = File.OpenRead(Path);
        wk = new XSSFWorkbook(fs);

        // creating bottom pages

        for (int i = 1; i < wk.NumberOfSheets; i++)
        {
            pages.CreateSheet(wk.GetSheetAt(i).SheetName, i);
        }

        LoadSheet(PlayerPrefs.GetInt("Index") == 0 ? 1 : PlayerPrefs.GetInt("Index"));
    }

    public void LoadSheet(int index)
    {
        ClearSheet();

        PlayerPrefs.SetInt("Index", index);

        if (index > wk.NumberOfSheets)
            index = 1;

        sheet = wk.GetSheetAt(index);

        Day curDay = NewDay();

        row = sheet.GetRow(6);

        curDay.Label(row.GetCell(47).StringCellValue +
                    "\n" + row.GetCell(48).StringCellValue);

        for (int i = 7; i <= sheet.LastRowNum; i++)
        {
            row = sheet.GetRow(i);

            if (row == null) continue;

            if (!string.IsNullOrEmpty(row.GetCell(47).StringCellValue))
            {
                curDay = NewDay();
                curDay.Label(row.GetCell(47).StringCellValue +
                    "\n" + row.GetCell(48).StringCellValue);
            }

            if (string.IsNullOrEmpty(row.GetCell(51).StringCellValue)) continue;

            string number = row.GetCell(49).NumericCellValue.ToString();
            string time = row.GetCell(50).StringCellValue;
            string lesson = row.GetCell(51).StringCellValue;

            curDay.AddLesson($"№{number} {time}", lesson);
        }
    }

    private Day NewDay()
    {
        var obj = Instantiate(dayPrefab, parent, false);

        return obj.GetComponent<Day>();
    }

    private void ClearSheet()
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        { 
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        Loader.FileLoaded -= FileLoaded;
        StopAllCoroutines();
    }

    private IEnumerator InfoCor()
    { 
        infoTxt.SetActive(true);

        yield return new WaitForSeconds(2);

        infoTxt.SetActive(false);
    }
}
