using System.Collections;
using System.Collections.Generic;
namespace MJGame
{

    public class AudioTest : SingletonComponent<AudioTest>
    {
        [Button]
        public void VFX()
        {
            AudioManager.Instance.PlayVFX("Reload");
        }

        [Button]
        public void SFX1()
        {
            AudioManager.Instance.PlaySFX("ClickButton");
        }

        [Button]
        public void SFX2()
        {
            AudioManager.Instance.PlaySFX("PlayerJump");
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