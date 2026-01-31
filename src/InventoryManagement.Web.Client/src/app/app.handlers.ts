import { Provider } from '@angular/core';
import { CreateIngredientHandler } from './application/ingredients/create/create-ingredient.handler';

// Import handlers here

export const APPLICATION_HANDLERS: Provider[] = [
  CreateIngredientHandler
];

