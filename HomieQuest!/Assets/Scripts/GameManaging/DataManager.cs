using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager {
    private static DataManager _instance;

    public static DataManager Instance {
        get {
            if (_instance == null)
                _instance = new DataManager();
            return _instance;
        }
    }
    private DataManager() {

    }


    #region Fields ================================================================================
    #endregion

    public class Data {
        public int level, maxHp, lives; // level can be out of valid range, check while loading
        public int powerUps; // 0 through 3

        public Data(int level,int maxHp, int lives, int powerUps) {
            this.level = level; this.maxHp = maxHp; this.lives = lives; this.powerUps = powerUps; // set all varriables
        }
    }
    public Data data { get; private set; }

    public void SaveData(Data newData) {
        data = newData;
        int level = SceneManager.GetActiveScene().buildIndex + 1; // level can be out of valid range, check while loading

        // save to disk TODO
    }

    public Data LoadData() {
        return data;
    }


    #region Public Methods ================================================================================
	#endregion
	
    #region Private Methods ================================================================================
	#endregion
}
