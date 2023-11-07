using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject dayPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private BottomPages pages;

    [SerializeField] private GameObject infoTxt;

    [SerializeField] private GameObject loadMenu;

    public static GameManager inst;

    public static string Path;

    private static string uni = "<h4>Гуманитарно-педагогический институт</h4>";

    private async void Awake()
    {
        inst = this;

        Path = Application.persistentDataPath + "/file.xlsx";

        // 2 PIN, 83 Language
        Loader.FileLoaded += FileLoaded;
        await Loader.GetFile(uni);
    }

    private XSSFWorkbook wk;
    private FileStream fs;

    private ISheet sheet;           // Рабочий лист
    private IRow row;               //Строка

    private void FileLoaded(bool val)
    {
        Loader.FileLoaded -= FileLoaded;

        Destroy(loadMenu);

        StartCoroutine(InfoCor(val));

        if (!File.Exists(Path)) return;

        fs = File.OpenRead(Path);
        wk = new XSSFWorkbook(fs);

        // creating bottom pages

        int index = PlayerPrefs.GetInt("Index") == 0 ? 1 : PlayerPrefs.GetInt("Index");

        if (index > wk.NumberOfSheets)
            index = 1;

        for (int i = 1; i < wk.NumberOfSheets; i++)
        {
            pages.CreateSheet(wk.GetSheetAt(i).SheetName, i, index == i ? true : false);
        }

        LoadSheet(index);
    }

    public void LoadSheet(int index)
    {
        ClearSheet();

        PlayerPrefs.SetInt("Index", index);

        sheet = wk.GetSheetAt(index);

        Day curDay = NewDay();

        bool skipLesson = true;

        for (int i = sheet.LastRowNum; i >= 6; i--)
        {
            row = sheet.GetRow(i);

            if (row == null) continue;

            string lesson = string.Empty;

            if (row.GetCell(51) != null && !string.IsNullOrEmpty(row.GetCell(51).StringCellValue))
            {
                string lessonType = row.GetCell(52).StringCellValue.Replace("\n", " ");

                if (lessonType.Contains("Л"))
                    lessonType = "<color=blue>" + lessonType;
                else
                    lessonType = "<color=red>" + lessonType;

                lesson = row.GetCell(51).StringCellValue + " " + lessonType;
                skipLesson = false;
            }

            if (skipLesson) goto check;

            string number = row.GetCell(49).NumericCellValue.ToString();
            string time = row.GetCell(50).StringCellValue;

            curDay.AddLesson($"№{number}\n{time}", lesson);

        check:
            if (!string.IsNullOrEmpty(row.GetCell(47).StringCellValue))
            {
                curDay.Label(row.GetCell(47).StringCellValue +
                    "\n" + row.GetCell(48).StringCellValue);

                if (i > 10)
                {
                    curDay = NewDay();
                    skipLesson = true;
                }
            }
        }
    }

    private Day NewDay()
    {
        var obj = Instantiate(dayPrefab, parent, false);

        obj.transform.SetAsFirstSibling();

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

    private IEnumerator InfoCor(bool val)
    {
        if (!val)
            infoTxt.GetComponent<TMP_Text>().text = "Не удалось скачать";

        infoTxt.SetActive(true);

        yield return new WaitForSeconds(2);

        infoTxt.SetActive(false);
    }
}
