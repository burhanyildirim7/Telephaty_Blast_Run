using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;

    private Animator anim;

    [Header("KaraktereDonusYaptirmakIcinGerekli")]
    KarakterPaketiMovement karakterPaketiMovement;

    private void Awake()
    {
        if (instance == null) instance = this;
        //else Destroy(this);
    }

    void Start()
    {
        StartingEvents();
    }

    /// <summary>
    /// Playerin collider olaylari.. collectible, engel veya finish noktasi icin. Burasi artirilabilir.
    /// elmas icin veya baska herhangi etkilesimler icin tag ekleyerek kontrol dongusune eklenir.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("collectible"))
        {
            // COLLECTIBLE CARPINCA YAPILACAKLAR...
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku

        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
            {
                // FAİL EVENTLERİ BURAYA YAZILACAK..
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen(); // Bu fonksiyon direk çağrılada bilir veya herhangi bir effect veya animasyon bitiminde de çağrılabilir..
                                                             // oyuncu fail durumunda bu fonksiyon çağrılacak.. 
            }
        }
        else if (other.CompareTag("finish"))
        {
            StartCoroutine(KosuyuSonlandırGeciktir());
            // finishe collider eklenecek levellerde...
            // FINISH NOKTASINA GELINCE YAPILACAKLAR... Totalscore artırma, x işlemleri, efektler v.s. v.s.
            //GameController.instance.isContinue = false;
            //GameController.instance.ScoreCarp(7);  // Bu fonksiyon normalde x ler hesaplandıktan sonra çağrılacak. Parametre olarak x i alıyor. 
            // x değerine göre oyuncunun total scoreunu hesaplıyor.. x li olmayan oyunlarda parametre olarak 1 gönderilecek.
            //UIController.instance.ActivateWinScreen(); // finish noktasına gelebildiyse her türlü win screen aktif edilecek.. ama burada değil..
            // normal de bu kodu x ler hesaplandıktan sonra çağıracağız. Ve bu kod çağrıldığında da kazanılan puanlar animasyonlu şekilde artacak..   
        }
        else if (other.CompareTag("enemy"))
        {
            KosuyuSonlandır();

            GameController.instance.isContinue = false;
            UIController.instance.ActivateLooseScreen();

        }
        else if (other.CompareTag("DonusYap"))
        {
            karakterPaketiMovement.KaraktereDonusYaptir();
        }
    }

    IEnumerator KosuyuSonlandırGeciktir()
    {
        karakterPaketiMovement._speed = 9;

        yield return new WaitForSeconds(1.5f);
        KosuyuSonlandır();
        GameController.instance.isContinue = false;
        UIController.instance.ActivateWinScreen();
        GameController.instance.ScoreCarp(1);
    }

    public void KosuyaBasla()
    {
        anim.SetBool("KosmaP", true);
    }

    public void KosuyuSonlandır()
    {
        anim.SetBool("KosmaP", false);
    }

    public void ObjeleriKontrolEtBasla()
    {
        anim.SetBool("ObjeKontrolEtmeP", true);
    }

    public void ObjeleriKontrolEtBitir()
    {
        anim.SetBool("ObjeKontrolEtmeP", false);
    }

    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        StartCoroutine(AnimasyonAlgilayici());
        karakterPaketiMovement = GameObject.FindWithTag("KarakterPaketi").GetComponent<KarakterPaketiMovement>();
        
       
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;

    }

    IEnumerator AnimasyonAlgilayici()
    {
        yield return new WaitForSeconds(.08f);
        for (int i = 0; i < 3; i++)
        {
            if (transform.GetChild(i).transform.gameObject.activeSelf)
            {
                anim = transform.GetChild(i).GetComponent<Animator>();
            }
        }
    }

}
