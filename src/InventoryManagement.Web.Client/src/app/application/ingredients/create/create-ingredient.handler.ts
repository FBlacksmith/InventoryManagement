import { RequestHandler, requestHandler, RequestData } from 'mediatr-ts';
import { Injectable, inject } from '@angular/core';
import { IngredientApiService } from '@infra/services/ingredient-api.service';
import { CreateIngredientSchema } from './create-ingredient.schema';

export class CreateIngredientRequest extends RequestData<void> implements CreateIngredientSchema {
  name: string;
  measurementUnit: any;

  constructor(data: CreateIngredientSchema) {
    super();
    this.name = data.name;
    this.measurementUnit = data.measurementUnit;
  }
}

@Injectable({ providedIn: 'root' })
@requestHandler(CreateIngredientRequest)
export class CreateIngredientHandler implements RequestHandler<CreateIngredientRequest, void> {
  private _api = inject(IngredientApiService);

  async handle(request: CreateIngredientRequest): Promise<void> {
    this._api.createIngredient({
      name: request.name,
      measurementUnit: request.measurementUnit
    });
  }
}
