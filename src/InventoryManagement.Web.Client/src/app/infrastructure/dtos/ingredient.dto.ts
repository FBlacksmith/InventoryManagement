export interface IngredientDTO {
  id: string;
  name: string;
  measurementUnit: string;
}

export interface CreateIngredientRequest {
  name: string;
  measurementUnit: string;
}
