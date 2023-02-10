using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntity;
using UnityEngine.SceneManagement;

public class Portal : Entity
{
    public override void Recovery(int amount)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    [SerializeField] private string nextLevel;
    public void LoadScene()
    {
        SceneManager.LoadScene(nextLevel);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = TilemapReader.RepositioningTheWorld(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
