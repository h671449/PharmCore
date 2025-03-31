using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace PharmCore.Client.Pages.Components
{
    public class Concept
    {
        public int Number { get; set; }
         
        public List<string> Labels { get; set; }
        public string Url { get; set; }
        public double Hue { get; set; }

        //public float Opacity { get; set; }

        private int NrInCategory { get; set; }

        private readonly double dynamicsBaseHue = 2;    //  7    steps at  7   = 49
        private readonly double kineticsBaseHue = 195;  //  9    steps at  5   = 245
        private readonly double outcomesBaseHue = 286;  //  5    steps at  8   = 320

        private readonly double hueStep = 8;

        private readonly double dynamicsHueStep = 6.0;
        private readonly double kineticsHueStep = 3.8;
        private readonly double outcomesHueStep = 9.0;
        

        public Concept(int number, int nrInCategory, List<String> labels, string url, string category)
        {
            Number = number;
            NrInCategory = nrInCategory;
            Labels = labels;
            Url = url;
            Hue = category switch
            {
                "Kinetics" => kineticsBaseHue + (NrInCategory - 1) * kineticsHueStep,
                "Dynamics" => dynamicsBaseHue + (NrInCategory - 1) * dynamicsHueStep,
                "Outcomes" => outcomesBaseHue + (NrInCategory - 1) * outcomesHueStep,
                _ => 0
            };
            //Opacity = string.IsNullOrEmpty(url) ? 0.4f : 1.0f;
        }
    }

    public static class ConceptFactory
    {
        //private static Concept inert = new Concept(0, 0, new List<string> { "" }, "", "");
        

        public static List<Concept> GetConcepts()
        {
            return new List<Concept>
            {

                new Concept(14, 5, new List<string> { "Drug", "Metabolism" }, "/Concepts/DrugMetabolism", "Kinetics"),
                new Concept(15, 6, new List<string> { "Zero- and", "First-Order", "Kinetics" }, "/Concepts/ZeroAndFirstOrderKinetics", "Kinetics"),
                new Concept(16, 7, new List<string> { "Drug", "Elimination" }, "/Concepts/DrugElimination", "Kinetics"),
                new Concept(17, 8, new List<string> { "Drug", "Elimination", "Half-Life" }, "/Concepts/DrugEliminationHalfLife", "Kinetics"),
                new Concept(18, 9, new List<string> { "Drug", "Clearance" }, "/Concepts/DrugClearance", "Kinetics"),
                new Concept(19, 10, new List<string> { "Steady-State", "Concentration" }, "/Concepts/SteadyStateConcentration", "Kinetics"),

                new Concept(5, 1, new List<string> { "Dose/", "Concentration-", "Response", "Relationship" }, "/Concepts/DoseConcentrationResponseRelationship", "Outcomes"),
                new Concept(20, 2, new List<string> { "Drug", "Tolerance" }, "/Concepts/DrugTolerance", "Outcomes"),
                new Concept(21, 3, new List<string> { "Adverse", "Drug", "Reaction" }, "/Concepts/AdverseDrugReaction", "Outcomes"),
                new Concept(22, 4, new List<string> { "Therapeutic", "Index" }, "/Concepts/TherapeuticIndex", "Outcomes"),
                new Concept(23, 5, new List<string> { "Drug", "Interaction" }, "/Concepts/DrugInteraction", "Outcomes"),
                new Concept(24, 6, new List<string> { "Individual Variation", "in Drug Response" }, "/Concepts/IndividualVariationInDrugResponse", "Outcomes"),

                new Concept(1, 1, new List<string> { "Drug", "Target" }, "/Concepts/DrugTarget", "Dynamics"),
                new Concept(2, 2, new List<string> { "Drug-Target", "Interaction" }, "/Concepts/DrugTargetInteraction", "Dynamics"),
                new Concept(3, 3, new List<string> { "Structure-","Activity", "Relationship" }, "/Concepts/StructureActivityRelationship", "Dynamics"),
                new Concept(4, 4, new List<string> { "Mechanism", "of Drug Action" }, "/Concepts/MechanismOfDrugAction", "Dynamics"),
                new Concept(6, 5, new List<string> { "Affinity" }, "/Concepts/Affinity", "Dynamics"),
                new Concept(7, 6, new List<string> { "Efficacy" }, "/Concepts/Efficacy", "Dynamics"),
                new Concept(8, 7, new List<string> { "Potency" }, "/Concepts/Potency", "Dynamics"),
                new Concept(9, 8, new List<string> { "Drug", "Selectivity" }, "/Concepts/DrugSelectivity", "Dynamics"),

                new Concept(10, 1, new List<string> { "Drug", "Absorption" }, "/Concepts/DrugAbsorption", "Kinetics"),
                new Concept(11, 2, new List<string> { "Drug", "Bioavailability" }, "/Concepts/DrugBioavailability", "Kinetics"),
                new Concept(12, 3, new List<string> { "Drug", "Distribution" }, "/Concepts/DrugDistribution", "Kinetics"),
                new Concept(13, 4, new List<string> { "Volume of", "Distribution" }, "/Concepts/VolumeOfDistribution", "Kinetics"),

            };
        }

        public static List<Concept> GetConceptsDD()
        {
            return new List<Concept>
            {

                new Concept(14, 5, new List<string> { "Drug", "Metabolism" }, "/Concepts/DrugMetabolism", "Kinetics"),
                new Concept(15, 6, new List<string> { "Zero- and First-", "Order Kinetics" }, "/Concepts/ZeroAndFirstOrderKinetics", "Kinetics"),
                new Concept(16, 7, new List<string> { "Drug", "Elimination" }, "/Concepts/DrugElimination", "Kinetics"),
                new Concept(17, 8, new List<string> { "Drug", "Elimination Half-Life" }, "/Concepts/DrugEliminationHalfLife", "Kinetics"),
                new Concept(18, 9, new List<string> { "Drug", "Clearance" }, "/Concepts/DrugClearance", "Kinetics"),
                new Concept(19, 10, new List<string> { "Steady-State", "Concentration" }, "/Concepts/SteadyStateConcentration", "Kinetics"),

                new Concept(5, 1, new List<string> { "Dose/Concentration-", "Response Relationship" }, "/Concepts/DoseConcentrationResponseRelationship", "Outcomes"),
                new Concept(20, 2, new List<string> { "Drug", "Tolerance" }, "/Concepts/DrugTolerance", "Outcomes"),
                new Concept(21, 3, new List<string> { "Adverse", "Drug", "Reaction" }, "/Concepts/AdverseDrugReaction", "Outcomes"),
                new Concept(22, 4, new List<string> { "Therapeutic", "Index" }, "/Concepts/TherapeuticIndex", "Outcomes"),
                new Concept(23, 5, new List<string> { "Drug", "Interaction" }, "/Concepts/DrugInteraction", "Outcomes"),
                new Concept(24, 6, new List<string> { "Individual Variation", "in Drug Response" }, "/Concepts/IndividualVariationInDrugResponse", "Outcomes"),

                new Concept(0, 0, new List<string> { "" }, "", ""),
                new Concept(0, 0, new List<string> { "" }, "", ""),

                new Concept(1, 1, new List<string> { "Drug", "Target" }, "/Concepts/DrugTarget", "Dynamics"),
                new Concept(2, 2, new List<string> { "Drug-Target", "Interaction" }, "/Concepts/DrugTargetInteraction", "Dynamics"),
                new Concept(3, 3, new List<string> { "Structure-","Activity Relationship" }, "/Concepts/StructureActivityRelationship", "Dynamics"),
                new Concept(4, 4, new List<string> { "Mechanism", "of Drug Action" }, "/Concepts/MechanismOfDrugAction", "Dynamics"),
                new Concept(6, 5, new List<string> { "Affinity" }, "/Concepts/Affinity", "Dynamics"),
                new Concept(7, 6, new List<string> { "Efficacy" }, "/Concepts/Efficacy", "Dynamics"),
                new Concept(8, 7, new List<string> { "Potency" }, "/Concepts/Potency", "Dynamics"),
                new Concept(9, 8, new List<string> { "Drug", "Selectivity" }, "/Concepts/DrugSelectivity", "Dynamics"),

                new Concept(10, 1, new List<string> { "Drug", "Absorption" }, "/Concepts/DrugAbsorption", "Kinetics"),
                new Concept(11, 2, new List<string> { "Drug", "Bioavailability" }, "/Concepts/DrugBioavailability", "Kinetics"),
                new Concept(12, 3, new List<string> { "Drug", "Distribution" }, "/Concepts/DrugDistribution", "Kinetics"),
                new Concept(13, 4, new List<string> { "Volume of", "Distribution" }, "/Concepts/VolumeOfDistribution", "Kinetics"),

                new Concept(0, 0, new List<string> { "" }, "", ""),
                new Concept(0, 0, new List<string> { "" }, "", ""),
            };
        }
    }
}