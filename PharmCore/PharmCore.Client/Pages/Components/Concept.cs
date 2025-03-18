using System.Collections.Generic;
using System.Drawing;

namespace PharmCore.Client.Pages.Components
{
    public class Concept
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public Color Color { get; set; }
        public float Opacity { get; set; }

        public Concept(string label, string url, string category)
        {
            Label = label;
            Url = url;
            Color = category switch
            {
                "Pharmacokinetics" => Color.DarkCyan,
                "Pharmacodynamics" => Color.DarkOrange,
                "Patient Outcomes" => Color.RebeccaPurple,
                _ => Color.Gray
            };
            Opacity = string.IsNullOrEmpty(url) ? 0.4f : 1.0f;
        }
    }

    public static class ConceptFactory
    {
        public static List<Concept> GetConcepts()
        {
            return new List<Concept>
            {
                new Concept("Drug Target (1)", "", "Pharmacodynamics"),
                new Concept("Drug-Target Interaction (2)", "", "Pharmacodynamics"),
                new Concept("Structure-Activity Relationship (3)", "", "Pharmacodynamics"),
                new Concept("Mechanism of Drug Action (4)", "", "Pharmacodynamics"),
                new Concept("Dose/Concentration-Response Relationship (5)", "", "Pharmacodynamics"),
                new Concept("Affinity (6)", "", "Pharmacodynamics"),
                new Concept("Efficacy (7)", "", "Pharmacodynamics"),
                new Concept("Potency (8)", "", "Pharmacodynamics"),
                new Concept("Drug Selectivity (9)", "", "Pharmacodynamics"),

                new Concept("Drug Absorption (10)", "", "Pharmacokinetics"),
                new Concept("Drug Bioavailability (11)", "", "Pharmacokinetics"),
                new Concept("Drug Distribution (12)", "", "Pharmacokinetics"),
                new Concept("Volume of Distribution (13)", "/Concepts/VolumeOfDistribution", "Pharmacokinetics"),
                new Concept("Drug Metabolism (14)", "", "Pharmacokinetics"),
                new Concept("Zero- and First-Order Kinetics (15)", "", "Pharmacokinetics"),
                new Concept("Drug Elimination (16)", "", "Pharmacokinetics"),
                new Concept("Drug Elimination Half-Life (17)", "", "Pharmacokinetics"),
                new Concept("Drug Clearance (18)", "", "Pharmacokinetics"),
                new Concept("Steady-State Concentration (19)", "", "Pharmacokinetics"),

                new Concept("Drug Tolerance (20)", "", "Patient Outcomes"),
                new Concept("Adverse Drug Reaction (21)", "", "Patient Outcomes"),
                new Concept("Therapeutic Index (22)", "", "Patient Outcomes"),
                new Concept("Drug Interaction (23)", "", "Patient Outcomes"),
                new Concept("Individual Variation in Drug Response (24)", "", "Patient Outcomes")
            };
        }
    }
}