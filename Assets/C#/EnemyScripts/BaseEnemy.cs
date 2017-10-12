using UnityEngine;
using System.Collections;

abstract public class BaseEnemy : Hittable {

	public ProbabililtyItem [] probabilityDrops;    //Array of possible drops for this enemy
    public GameObject[] attachedDrops;              // Things an enemy may be carrying that we want to detach, and let loose when death
    public Transform healthBar;                    // An object that we scale based off of health
    public ParticleSystem deathParticles;           // Particles when we ded, optional



    public int minDrops;			//Mininum number of possible drops
	public int maxDrops;			//Maximum number of possible drops
	public float health;			//Enemy health
	public float movementSpeed;		//Enemy movement speed

    private int deathLayer = 14;



    /*
	 * Method called when enemy dies
	 * Identifies the number of drops the enemy drops based on the minimum and maximum values
	 * Then, for each of the individual drops, find a random GameObject in the drops array to drop
	 * Instantiate that drop
	 */
    void GenericDeath(){

        OnDeath();

        healthBar.gameObject.SetActive(false);

        if (deathParticles != null) {
            deathParticles.Play();
        }

        // Move to layer where we won't collide with weapons or the player
        MoveToLayer(transform, deathLayer);
        // Prevent from moving
        //TODO ragdoll
        this.GetComponent<Rigidbody>().freezeRotation = true;

        ArrayList drops = new ArrayList();
        for (int i = 0; i < probabilityDrops.Length; i++) {
            for (int k = 0; k < probabilityDrops[i].chance; k++) {
                drops.Add(i);
            }
        }

        int numberOfDrops = Mathf.RoundToInt (Random.Range (minDrops, maxDrops));
        print("dropping " + numberOfDrops + " drops");
		for (int i = 0; i < numberOfDrops; i++) {
			int dropIndex = Mathf.RoundToInt (Random.Range (0, drops.Count));
            GameObject spawn = GameObject.Instantiate(probabilityDrops[(int)drops[dropIndex]].prefab, transform.position, Quaternion.Euler(360 * Random.insideUnitSphere));
            ItemStats it;
            if (it = spawn.GetComponent<ItemStats>()) {
                // Somewhere a bit above min condition to max condition
                it.condition = Random.Range((0.3f * (it.maxCondition - it.minCondition)) + it.minCondition, it.maxCondition);
            }
        }

        foreach (GameObject attached in attachedDrops) {
            attached.transform.parent = null;
            Rigidbody rig = attached.GetComponent<Rigidbody>();
            rig.isKinematic = false;
            rig.velocity = ((Random.insideUnitSphere + (Vector3.up * 1)) * 2);
            rig.AddTorque((Random.insideUnitSphere * 360));
            foreach (Collider col in attached.GetComponentsInChildren<Collider>())
                col.enabled = true;
            ItemStats it;
            if (it = attached.GetComponent<ItemStats>()) {
                it.condition = Random.Range((0.3f * (it.maxCondition - it.minCondition)) + it.minCondition, it.maxCondition);
            }
        }
	}

    public void MoveToLayer(Transform root, int layer) {
        // Recursively move all children and self to layer
        if (root.CompareTag("ResistSetLayer")) return;
        root.gameObject.layer = layer;
        foreach (Transform child in root) {
            MoveToLayer(child, layer);
        }
    }

    /*
	 * The damage done to the enemies health based on the parameter defined damage
	 */
    public override void Hit(float damage, Vector3 direction, DamageType type) {
        //print("enemy hit");
        // TODO add enemy vulnerabilities for damage types
        float originalHealth = health;
        if (health - damage > 0) {
            health -= damage;
        } else {
            health = 0;
        }
        healthBar.localScale = new Vector3(health, 1, 1);
        //healthBar.GetComponent<MeshRenderer>().materials[0].mainTextureScale = new Vector2(health / 5, 1); 
        if (health <= 0 && originalHealth > 0) {
            GenericDeath();
        } else {
            OnDamage(damage, type);
        }
	}

	/*
	 * Each frame, call enemies movement method if it's not attacking and attack method if it is
	 * Also check if health is below or equal to zero and call OnDeath if true
	 * note: has to be public because it needs to be in order for the sub-class to use it
	 */
	public void Update () {
		if (health > 0) Movement ();
        else {

        }
	}


   

	abstract public IEnumerator Attack();		//Abstract method that checks if the player is within range and then damages player
	abstract public void Movement();			//Abstract method defining how the specific enemy moves 
    abstract public void OnDeath();             //What to do when the enemy dies
    abstract public void OnDamage(float damage, DamageType type);            //What do to when the enemy takes damage
}
