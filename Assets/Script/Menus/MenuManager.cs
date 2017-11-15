using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public List<Menu> menus = new List<Menu>();
    public GameObject buttonPrefab;
    Menu _currentMenu = null;

    // Unity lifecycle
    void Start()
    {
        AudioManager.Play("Main Theme", false, "music");

        _currentMenu = GetMenu("main");
        _currentMenu.Initialize();
    }

    // Functions
    Menu GetMenu(string id)
    {
        foreach (Menu menu in menus)
            if (menu.id == id)
                return menu;

        return null;
    }

    public void GoTo(string menu)
    {
        Menu newMenu = null;
        if ((newMenu = GetMenu(menu)) != null)
            StartCoroutine("GoToMenu", newMenu);        
    }

    IEnumerator GoToMenu(Menu menu)
    {
        _currentMenu.Hide();

        do
        {
            yield return new WaitForSeconds(0.1f);
        } while (_currentMenu.visible);

        _currentMenu = menu;


        menu.Show();


        GenerateButtons(_currentMenu);
    }

    void SetSelectedWorld(int id)
    {
        MenuDatas.Instance.selectedWorld = id;
    }

    void LoadLevel(string levelResource)
    {
        AudioManager.StopByTag("music");
        MenuDatas.Instance.selectedLevel = levelResource;
        SceneManager.LoadScene(1);
    }

    void GenerateButtons(Menu menu)
    {
        if (menu.buttonContainer == null)
            return;

        List<World> buttonList = MenuDatas.Instance.worlds;

        if (menu.id == "world")
        {
            foreach (World world in buttonList)
            {
                GameObject goButton = Instantiate(buttonPrefab, menu.buttonContainer);
                var buttonScript = goButton.GetComponent<Button>();
                var text = buttonScript.GetComponentInChildren<Text>();
                buttonScript.interactable = !world.isLocked;
                text.text = world.name;

                buttonScript.onClick.AddListener(() => SetSelectedWorld(world.id));
                buttonScript.onClick.AddListener(() => GoTo("level"));
            }
        }
        else if (menu.id == "level")
        {
            var world = MenuDatas.Instance.GetWorld(MenuDatas.Instance.selectedWorld);

            if (world == null)
                return;

            foreach (Level level in world.levels)
            {
                GameObject goButton = Instantiate(buttonPrefab, menu.buttonContainer);
                var buttonScript = goButton.GetComponent<Button>();
                var text = buttonScript.GetComponentInChildren<Text>();
                buttonScript.interactable = !level.isLocked;
                text.text = "Niveau " + level.name;


                buttonScript.onClick.AddListener(() => LoadLevel(level.resourceName));
            }
        }
    }
    
    // TODO : Charger le bon niveau en fonction du bouton cliqué.
    // TODO : Créer les murs de la pièce + texturing.
    // TODO : Créer texture parchemin pour support plateau.

}
