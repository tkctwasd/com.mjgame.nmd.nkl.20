using System.Collections;
using System.Collections.Generic;
namespace MJGame
{

    public class AudioTest : SingletonComponent<AudioTest>
    {
        [Button]
        public void VFX()
        {
            AudioManager.Instance.PlayVFX("MonsterWarning");
        }

        [Button]
        public void SFX()
        {
            AudioManager.Instance.PlaySFX("ClickButton");
        }


        [Button]
        public void BG1()
        {
            AudioManager.Instance.PlayBGMusic("Chapter1");
        }

        [Button]
        public void BG2()
        {
            AudioManager.Instance.PlayBGMusic("Chapter2");
        }

       
    }

}