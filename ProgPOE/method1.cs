using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPOE
{
    // Class representing an ingredient with additional properties
    public class Ingredient
    {
        public string Name { get; set; } // Name of the ingredient
        public double Quantity { get; set; } // Quantity of the ingredient
        public string Unit { get; set; } // Unit of measurement
        public double Calories { get; set; } // Number of calories
        public string FoodGroup { get; set; } // Food group

        // Constructor to initialize an Ingredient object
        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }

        // Override toString method to display ingredient details
        public override string ToString()
        {
            return $"Ingredient: {Name}, Quantity: {Quantity} {Unit}, Calories: {Calories}, Food Group: {FoodGroup}";
        }
    }

    // Class representing a recipe
    public class Recipe
    {
        public string Name { get; set; } // Name of the recipe
        public List<Ingredient> Ingredients { get; } // List of ingredients
        public List<string> Steps { get; } // List of steps
        private List<double> OriginalQuantities { get; } // List to store original quantities for resetting

        // Constructor to initialize a Recipe object
        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
            OriginalQuantities = new List<double>();
        }

        // Method to add an ingredient and store its original quantity
        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
            OriginalQuantities.Add(ingredient.Quantity);
        }

        // Method to calculate total calories
        public double TotalCalories()
        {
            return Ingredients.Sum(ingredient => ingredient.Calories);
        }

        // Method to check if total calories exceed 300
        public bool ExceedsCalorieLimit()
        {
            return TotalCalories() > 300;
        }

        // Method to reset quantities to original values
        public void ResetQuantities()
        {
            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredients[i].Quantity = OriginalQuantities[i];
            }
        }

        // Override toString method to display recipe name
        public override string ToString()
        {
            return Name;
        }
    }

    // Class containing methods to manage recipes
    public class method1
    {
        private List<Recipe> recipes; // List of recipes

        // Constructor to initialize method1 object
        public method1()
        {
            recipes = new List<Recipe>();
        }

        // Method to display menu and handle user input
        public void menu()
        {
            Console.WriteLine("Please enter your name:"); // Asking user for their name
            string name = Console.ReadLine(); // Reads user's name

            Console.WriteLine($"\nHello {name}, please choose an option:");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("1. Add recipe");
                Console.WriteLine("2. Display recipes");
                Console.WriteLine("3. Scale recipe");
                Console.WriteLine("4. Reset quantities");
                Console.WriteLine("5. Clear recipes");
                Console.WriteLine("6. Exit");

                string input = Console.ReadLine(); // Read user's menu choice
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddRecipe(); // Calls method to add recipe
                        break;
                    case "2":
                        DisplayRecipes(); // Calls method to display recipes
                        break;
                    case "3":
                        ScaleRecipe(); // Calls method to scale recipe quantities
                        break;
                    case "4":
                        ResetQuantities(); // Calls method to reset ingredient quantities
                        break;
                    case "5":
                        ClearRecipes(); // Calls method to clear all recipes
                        break;
                    case "6":
                        exit = true; // Exits program
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again."); // Invalid option message
                        break;
                }
            }
        }

        // Method to add a new recipe
        private void AddRecipe()
        {
            Console.WriteLine("Enter recipe name:");
            string name = Console.ReadLine(); // Reads name of the recipe the user entered

            Recipe recipe = new Recipe(name);

            Console.WriteLine("\nEnter number of ingredients:");
            int numOfIngredients = Convert.ToInt32(Console.ReadLine()); // Gets number of ingredients

            for (int i = 0; i < numOfIngredients; i++)
            {
                Console.WriteLine($"\nIngredient {i + 1}");

                Console.Write("Name: ");
                string ingredientName = Console.ReadLine(); // Reads ingredient name

                Console.Write("Quantity: ");
                double quantity;
                while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0) // Validates quantity
                {
                    Console.Write("Please enter a valid quantity greater than 0: ");
                }

                Console.Write("Unit of measurement: ");
                string unit = Console.ReadLine(); // Gets unit of measurement

                Console.Write("Calories: ");
                double calories;
                while (!double.TryParse(Console.ReadLine(), out calories) || calories < 0) // Validates calories
                {
                    Console.Write("Please enter a valid number of calories: ");
                }

                Console.Write("Food group: ");
                string foodGroup = Console.ReadLine(); // Gets food group

                // Creates a new ingredient object and adds it to the recipe
                recipe.AddIngredient(new Ingredient(ingredientName, quantity, unit, calories, foodGroup));
            }

            Console.WriteLine("\nEnter number of steps:");
            int stepCount = Convert.ToInt32(Console.ReadLine()); // Gets number of steps

            for (int i = 0; i < stepCount; i++)
            {
                Console.WriteLine($"\nStep {i + 1}");
                Console.Write("Description: ");
                string description = Console.ReadLine(); // Reads the description
                recipe.Steps.Add(description); // Adds step to recipe
            }

            recipes.Add(recipe); // Adds recipe to the list

            // Check if total calories exceed 300 and notify the user
            if (recipe.ExceedsCalorieLimit())
            {
                Console.WriteLine("\nWarning: The total calories of this recipe exceed 300.");
            }

            Console.WriteLine("\nRecipe added successfully!");
        }

        // Method to display a list of recipes in alphabetical order
        private void DisplayRecipes()
        {
            if (recipes.Count == 0) // Checks if there are no recipes
            {
                Console.WriteLine("\nNo recipes available. Please add a recipe first.");
                return;
            }

            var sortedRecipes = recipes.OrderBy(r => r.Name).ToList(); // Sorts recipes by name

            Console.WriteLine("Recipes:");
            for (int i = 0; i < sortedRecipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedRecipes[i].Name}");
            }

            Console.WriteLine("\nEnter the number of the recipe to display its details:");
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > sortedRecipes.Count) // Validates choice
            {
                Console.WriteLine("Please enter a valid number corresponding to a recipe:");
            }

            DisplayRecipeDetails(sortedRecipes[choice - 1]); // Displays selected recipe details
        }

        // Method to display details of a selected recipe
        private void DisplayRecipeDetails(Recipe recipe)
        {
            Console.WriteLine("\n----------------------------------------------------------------------------");
            Console.WriteLine($"\nRecipe: {recipe.Name}\n");
            Console.WriteLine("----------------------------------------------------------------------------");

            Console.WriteLine("\nIngredients\n");
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.WriteLine(ingredient.ToString());
            }

            Console.WriteLine("----------------------------------------------------------------------------");

            Console.WriteLine("\nSteps\n");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                Console.WriteLine($"Step {i + 1}: {recipe.Steps[i]}");
            }

            Console.WriteLine("----------------------------------------------------------------------------");

            Console.WriteLine($"\nTotal Calories: {recipe.TotalCalories()}");
        }

        // Method to scale the quantities of ingredients in a recipe
        private void ScaleRecipe()
        {
            if (recipes.Count == 0) // Checks if there are no recipes
            {
                Console.WriteLine("\nNo recipes available. Please add a recipe first.");
                return;
            }

            Console.WriteLine("Enter the number of the recipe to scale:");
            DisplayRecipes();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > recipes.Count) // Validates choice
            {
                Console.WriteLine("Please enter a valid number corresponding to a recipe:");
            }

            Recipe recipe = recipes[choice - 1]; // Gets selected recipe

            Console.WriteLine("Enter scaling factor:");
            double factor;
            while (!double.TryParse(Console.ReadLine(), out factor) || factor <= 0) // Validates scaling factor
            {
                Console.WriteLine("Please enter a valid scaling factor greater than 0:");
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Quantity *= factor; // Scales ingredient quantities
            }

            Console.WriteLine($"\nRecipe scaled by a factor of {factor}.");
        }

        // Method to reset the quantities of ingredients in a recipe to their original values
        private void ResetQuantities()
        {
            if (recipes.Count == 0) // Checks if there are no recipes
            {
                Console.WriteLine("\nNo recipes available. Please add a recipe first.");
                return;
            }

            
            Console.WriteLine("Enter the number of the recipe to reset quantities:");
            DisplayRecipes();
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > recipes.Count) // Validates choice
            {
                Console.WriteLine("Please enter a valid number corresponding to a recipe:");
            }

            Recipe recipe = recipes[choice - 1]; // Gets selected recipe
            recipe.ResetQuantities(); // Resets quantities to original values

            Console.WriteLine("\nRecipe quantities have been reset to their original values.");
        }

        // Method to clear all recipes from the list
        private void ClearRecipes()
        {
            recipes.Clear(); // Clears the list of recipes
            Console.WriteLine("\nAll recipes have been cleared.");
        }
    }

  
}