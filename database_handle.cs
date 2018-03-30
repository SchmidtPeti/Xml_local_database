using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;

public class database_handle : MonoBehaviour {

    public struct score
    {
        public int player_score;
        public string jatekos_neve;
        public int hanyadik_szint;
        public bool felhasznalve;
    }
    public struct stage_db
    {
        public string stage_reached;
        public bool reached;
    }
    public struct coin
    {
        public int coin_count;
        public bool felhasznalva;
    }
    public struct name_struct
    {
        public string name;
    }
    public struct store
    {
        public int life;
        public int missle;
    }
    public struct piu_speed
    {
        public float add_speed;
    }
    List<score> collect_score;
    List<coin> coin_counter;
    List<name_struct> collect_name;
    List<store> store_name;
    List<stage_db> stage_db_list;
    List<piu_speed> piu_speed_list;
    XmlSerializer xml_file;
    // public string file_pah = Application.persistentDataPath + "score.xml";
    public string score_file_path = "score.xml";
    public string coin_file_path = "coin.xml";
    public string name_file_path = "name.xml";
    public string name_store_path = "store.xml";
    public string stage_Store_path = "stage.xml";
    public string piu_speed_path = "piu_speed.xml";
    public void add_score(int score,string jatekos_neve,int hanyadik_szint,bool felhasznalve)
    {
            FileStream create_my_xml = new FileStream(getPath(score_file_path), FileMode.Append, FileAccess.Write);
            collect_score = new List<score>();
            xml_file = new XmlSerializer(typeof(List<score>));
            score jelenlegi_adat = new score();
            jelenlegi_adat.player_score = score;
            jelenlegi_adat.jatekos_neve = jatekos_neve;
            jelenlegi_adat.hanyadik_szint = hanyadik_szint;
            jelenlegi_adat.felhasznalve = false;
            collect_score.Add(jelenlegi_adat);
            var ns = new XmlSerializerNamespaces();
            xml_file.Serialize(create_my_xml, collect_score,ns);
            create_my_xml.Close();
            more_node(score_file_path);
    }
    public void default_store_values()
    {
        FileStream create_my_xml = new FileStream(getPath(name_store_path), FileMode.Append, FileAccess.Write);
        store_name = new List<store>();
        xml_file = new XmlSerializer(typeof(List<store>));
        store jelenlegi_adat = new store();
        jelenlegi_adat.life = 3;
        jelenlegi_adat.missle = 0;
        store_name.Add(jelenlegi_adat);
        var ns = new XmlSerializerNamespaces();
        xml_file.Serialize(create_my_xml, store_name, ns);
        create_my_xml.Close();
        more_node(name_store_path);
    }
    public void add_life()
    {
        if (File.Exists(getPath(name_store_path)) == false)
        {
            default_store_values();
        }
        else
        {
            int new_life = get_life() + 1;
            int new_missle = get_missle();
            FileStream create_my_xml = new FileStream(getPath(name_store_path), FileMode.Append, FileAccess.Write);
            store_name = new List<store>();
            xml_file = new XmlSerializer(typeof(List<store>));
            store jelenlegi_adat = new store();
            jelenlegi_adat.life = new_life;
            jelenlegi_adat.missle = new_missle;
            store_name.Add(jelenlegi_adat);
            var ns = new XmlSerializerNamespaces();
            xml_file.Serialize(create_my_xml, store_name, ns);
            create_my_xml.Close();
            more_node(name_store_path);
        }

    }
    public int get_life()
    {
        int life_osszeg = 3;
        if (File.Exists(getPath(name_store_path)))
        {
            FileStream read_xml = new FileStream(getPath(name_store_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<store>));
            store_name = new List<store>();
            store_name = (List<store>)xml_file.Deserialize(read_xml);
            life_osszeg = store_name[store_name.Count - 1].life;
            read_xml.Close();
        }
        return life_osszeg;
    }
    public int get_missle()
    {
        int missle_osszeg = 0;
        if (File.Exists(getPath(name_store_path)))
        {
            FileStream read_xml = new FileStream(getPath(name_store_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<store>));
            store_name = new List<store>();
            store_name = (List<store>)xml_file.Deserialize(read_xml);
            missle_osszeg = store_name[store_name.Count - 1].missle;
            read_xml.Close();
        }
        return missle_osszeg;

    }
    public void add_missle()
    {
        if (File.Exists(getPath(name_store_path)) == false)
        {
            default_store_values();
        }
        else
        {
            int new_missle = get_missle() + 1; ;
            int new_life = get_life();
            FileStream create_my_xml = new FileStream(getPath(name_store_path), FileMode.Append, FileAccess.Write);
            store_name = new List<store>();
            xml_file = new XmlSerializer(typeof(List<store>));
            store jelenlegi_adat = new store();
            jelenlegi_adat.life = new_life;
            jelenlegi_adat.missle = new_missle;
            store_name.Add(jelenlegi_adat);
            var ns = new XmlSerializerNamespaces();
            xml_file.Serialize(create_my_xml, store_name, ns);
            create_my_xml.Close();
            more_node(name_store_path);
        }
    }
    public void more_node(string file_path)
    {
        List<string> sorok = new List<string>();
        List<string> vegso_sorok = new List<string>();
        StreamReader xml_writer = new StreamReader(getPath(file_path));
        while (!xml_writer.EndOfStream)
        {
            sorok.Add(xml_writer.ReadLine());
        }
        for (int i = 0; i < sorok.Count; i++)
        {
            if (i < 3 || i == sorok.Count - 1)
            {
                vegso_sorok.Add(sorok[i]);
            }
           else if(!sorok[i].StartsWith("</Array") && !sorok[i].StartsWith("<Array")&&!sorok[i].StartsWith("<?xml"))
            {
                vegso_sorok.Add(sorok[i]);
            }
        }
        xml_writer.Close();
        StreamWriter new_tartalom = new StreamWriter(getPath(file_path), false);
        for (int i = 0; i < vegso_sorok.Count; i++)
        {
            new_tartalom.WriteLine(vegso_sorok[i]);
        }
        new_tartalom.Close();
        //File.WriteAllLines(file_pah, vegso_sorok.ToArray());
    }
   // public void 
    public void add_coin(int osszeg, bool felhasznalva)
    {
        FileStream create_my_xml = new FileStream(getPath(coin_file_path), FileMode.Append, FileAccess.Write);
        coin_counter = new List<coin>();
        xml_file = new XmlSerializer(typeof(List<coin>));
        coin jelenlegi_adat = new coin();
        jelenlegi_adat.coin_count = osszeg;
        jelenlegi_adat.felhasznalva = false;
        coin_counter.Add(jelenlegi_adat);
        var ns = new XmlSerializerNamespaces();
        xml_file.Serialize(create_my_xml, coin_counter, ns);
        create_my_xml.Close();
        more_node(coin_file_path);
    }
    public void change_name(string name)
    {
        FileStream create_my_xml = new FileStream(getPath(name_file_path), FileMode.Append, FileAccess.Write);
        collect_name = new List<name_struct>();
        xml_file = new XmlSerializer(typeof(List<name_struct>));
        name_struct jelenlegi_adat = new name_struct();
        jelenlegi_adat.name = name;
        collect_name.Add(jelenlegi_adat);
        var ns = new XmlSerializerNamespaces();
        xml_file.Serialize(create_my_xml, collect_name, ns);
        create_my_xml.Close();
        more_node(name_file_path);
    }
    public void finish_Stage(string stage_name)
    {
        FileStream create_my_xml = new FileStream(getPath(stage_Store_path), FileMode.Append, FileAccess.Write);
        stage_db_list = new List<stage_db>();
        xml_file = new XmlSerializer(typeof(List<stage_db>));
        stage_db jelenlegi_adat = new stage_db();
        jelenlegi_adat.stage_reached = stage_name;
        jelenlegi_adat.reached = true;
        stage_db_list.Add(jelenlegi_adat);
        var ns = new XmlSerializerNamespaces();
        xml_file.Serialize(create_my_xml, stage_db_list, ns);
        create_my_xml.Close();
        more_node(stage_Store_path);
    }
    public bool reached_stage(string stage_name)
    {
        bool reach_any_stage = false;
        if (File.Exists(getPath(stage_Store_path)))
        {
            FileStream read_xml = new FileStream(getPath(stage_Store_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<stage_db>));
            stage_db_list = new List<stage_db>();
            stage_db_list = (List<stage_db>)xml_file.Deserialize(read_xml);
            for (int i = 0; i < stage_db_list.Count; i++)
            {
                if (stage_db_list[i].stage_reached == stage_name)
                {
                    reach_any_stage = stage_db_list[i].reached;
                }
            }
        }
        return reach_any_stage;
    }
    public string get_name()
    {
        string player_name = "";
        if (File.Exists(getPath(name_file_path)))
        {
            FileStream read_xml = new FileStream(getPath(name_file_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<name_struct>));
            collect_name = new List<name_struct>();
            collect_name = (List<name_struct>)xml_file.Deserialize(read_xml);
            player_name = collect_name[collect_name.Count - 1].name;
        }
        return player_name;

    }
    public int get_all_coin()
    {
        int osszeg = 0;
        if (File.Exists(getPath(coin_file_path)))
        {
            FileStream read_xml = new FileStream(getPath(coin_file_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<coin>));
            coin_counter = new List<coin>();
            coin_counter = (List<coin>)xml_file.Deserialize(read_xml);
            for (int i = 0; i < coin_counter.Count; i++)
            {
                if (!coin_counter[i].felhasznalva)
                {
                    osszeg+= coin_counter[i].coin_count;
                }
            }
            read_xml.Close();
        }
        return osszeg;
    }
    public int get_high_score()
    {
        int max = 0;
        if (File.Exists(getPath(score_file_path)))
        {
            FileStream read_xml = new FileStream(getPath(score_file_path), FileMode.Open,FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<score>));
            collect_score = new List<score>();
            collect_score = (List<score>)xml_file.Deserialize(read_xml);
            for (int i = 0; i < collect_score.Count; i++)
            {
                if (max < collect_score[i].player_score)
                {
                    max = collect_score[i].player_score;
                }

            }
            read_xml.Close();
        }
        return max;
    }
    public void add_speed(float speed)
    {
        float default_piu_speed = bullet.bullet_spead;
        ADD_ED_DFEULT:
        if (File.Exists(getPath(piu_speed_path)))
        {
            StreamWriter create_my_xml = new StreamWriter(getPath(piu_speed_path),true);
            piu_speed_list = new List<piu_speed>();
            xml_file = new XmlSerializer(typeof(List<piu_speed>));
            piu_speed jelenlegi_adat = new piu_speed();
            create_my_xml.Close();
            jelenlegi_adat.add_speed = float_get_bullet_speed()+speed;
            StreamWriter create_my_xml_again = new StreamWriter(getPath(piu_speed_path), true);
            piu_speed_list.Add(jelenlegi_adat);
            var ns = new XmlSerializerNamespaces();
            xml_file.Serialize(create_my_xml_again, piu_speed_list, ns);
            create_my_xml_again.Close();
            more_node(piu_speed_path);
        }
        else
        {
            FileStream create_my_xml = new FileStream(getPath(piu_speed_path), FileMode.Append, FileAccess.Write);
            piu_speed_list = new List<piu_speed>();
            xml_file = new XmlSerializer(typeof(List<piu_speed>));
            piu_speed jelenlegi_adat = new piu_speed();
            jelenlegi_adat.add_speed = default_piu_speed;
            piu_speed_list.Add(jelenlegi_adat);
            var ns = new XmlSerializerNamespaces();
            xml_file.Serialize(create_my_xml, piu_speed_list, ns);
            create_my_xml.Close();
            more_node(piu_speed_path);
            goto ADD_ED_DFEULT;
        }
    }
    public float float_get_bullet_speed()
    {
        float piu_speed_value = bullet.bullet_spead;
        if (File.Exists(getPath(piu_speed_path)))
        {
            FileStream read_xml = new FileStream(getPath(piu_speed_path), FileMode.Open, FileAccess.Read);
            xml_file = new XmlSerializer(typeof(List<piu_speed>));
            piu_speed_list = new List<piu_speed>();
            piu_speed_list = (List<piu_speed>)xml_file.Deserialize(read_xml);
            piu_speed_value = (float)piu_speed_list[piu_speed_list.Count-1].add_speed;
            read_xml.Close();
        }
        return piu_speed_value;
    }
    public void data_base_reset()
    {
        if (File.Exists(getPath(score_file_path)))
        {
            File.Delete(getPath(score_file_path));
        }
        if (File.Exists(getPath(coin_file_path)))
        {
            File.Delete(getPath(coin_file_path));
        }
    }
    private string getPath(string file_path)
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/" + file_path;
#elif UNITY_ANDROID
        return Application.persistentDataPath+file_path;
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+file_path;
#else
        return Application.dataPath +"/"+ file_path;
#endif
    }
}
