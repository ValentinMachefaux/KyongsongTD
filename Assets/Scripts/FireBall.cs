using Script;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector3 targetPosition;              // Cible de la boule de feu
    public float fallSpeed = 20f;               // Vitesse de la chute
    public float explosionRadius = 5f;          // Rayon d'explosion
    public float damage = 20f;                  // Dégâts infligés
    public float explosionHeightOffset = 10f;   // Hauteur de départ

    public AudioClip impactSound;               // Son d'impact
    public AudioSource audioSourcePrefab;       // Prefab audio source

    private bool hasExploded = false;           // Si la boule de feu a explosé
    private TrailRenderer trailRenderer;        // Pour gérer la traînée
    private GameObject trailObject;             // Objet de la traînée
    private GameObject explosionObject;         // Objet de l'explosion

    void Start()
    {
        // Initialiser la position de la boule de feu
        transform.position = new Vector3(targetPosition.x, targetPosition.y + explosionHeightOffset, targetPosition.z);

        // Créer les effets de traînée et d'explosion
        CreateTrailEffect();
    }

    void Update()
    {
        // Déplacer la boule de feu vers la cible
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

        // Déclencher l'explosion à l'impact
        if (!hasExploded && Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        // Créer l'effet d'explosion
        CreateExplosionEffect();
        PlayImpactSound();

        // Appliquer des dégâts
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Appliquer des dégâts aux ennemis
                }
            }
            else if (hit.CompareTag("Tower"))
            {
                Tower tower = hit.GetComponent<Tower>();
                if (tower != null)
                {
                    tower.TakeDamage(damage); // Appliquer des dégâts à la tour
                }
            }
            else if (hit.CompareTag("Base"))
            {
                Base baseObj = hit.GetComponent<Base>();
                if (baseObj != null)
                {
                    baseObj.TakeDamage(damage); // Appliquer des dégâts à la base
                }
            }
        }

        // Détruire la boule de feu après l'explosion
        Destroy(gameObject);
    }

    void PlayImpactSound()
    {
        if (impactSound != null && audioSourcePrefab != null)
        {
            AudioSource tempAudio = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
            tempAudio.clip = impactSound;
            tempAudio.Play();
            Destroy(tempAudio.gameObject, impactSound.length);
        }
    }

    // Méthode pour créer l'effet de traînée
    void CreateTrailEffect()
    {
        trailObject = new GameObject("TrailEffect");
        trailObject.transform.parent = transform;  // Attacher la traînée à la boule de feu

        // Créer un TrailRenderer pour la traînée
        trailRenderer = trailObject.AddComponent<TrailRenderer>();
        trailRenderer.startWidth = explosionRadius * 0.05f;  // Largeur initiale de la traînée (proportionnelle au rayon)
        trailRenderer.endWidth = explosionRadius * 0.02f;   // Largeur finale de la traînée
        trailRenderer.time = 0.5f;  // Durée de la traînée

        // Appliquer un matériau de base pour la traînée (peut être remplacé par un matériau de feu)
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = Color.red;  // Couleur de la traînée (rouge pour un effet de feu)
        trailRenderer.endColor = Color.yellow; // Couleur finale de la traînée (jaune pour simuler le feu qui s'estompe)
    }

    // Méthode pour créer l'effet d'explosion
    void CreateExplosionEffect()
    {
        // Créer l'objet de l'explosion
        explosionObject = new GameObject("ExplosionEffect");
        explosionObject.transform.position = transform.position;  // Placer l'explosion à la position de la boule de feu

        // Créer un ParticleSystem pour l'explosion
        ParticleSystem explosionSystem = explosionObject.AddComponent<ParticleSystem>();

        // Paramètres du ParticleSystem
        var mainModule = explosionSystem.main;
        mainModule.startSize = explosionRadius * 0.2f;  // Taille des particules (proportionnelle au rayon d'explosion)
        mainModule.startSpeed = explosionRadius * 0.3f;  // Vitesse des particules
        mainModule.startLifetime = 0.5f;                 // Durée de vie des particules
        mainModule.startColor = new Color(1f, 0.5f, 0f); // Couleur (orange/rouge pour un effet de feu)

        // Module d'émission
        var emissionModule = explosionSystem.emission;
        emissionModule.rateOverTime = explosionRadius * 10f; // Nombre de particules émises en fonction du rayon

        // Lancer l'explosion
        explosionSystem.Play();

        // Détruire l'objet d'explosion après un court délai pour libérer les ressources
        Destroy(explosionObject, 1f);  // Détruire l'explosion après 1 seconde pour qu'elle soit visible
    }

    // Méthode de visualisation du rayon d'explosion dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
