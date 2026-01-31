import { RequestHandler, requestHandler, RequestData } from 'mediatr-ts';
import { Injectable, inject } from '@angular/core';
import { IngredientApiService } from '@infra/services/ingredient-api.service';
import { CreateIngredientSchema } from './create-ingredient.schema';
import { IngredientDTO } from '@infra/dtos/ingredient.dto';

export class CreateIngredientRequest extends RequestData<IngredientDTO> implements CreateIngredientSchema {
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
export class CreateIngredientHandler implements RequestHandler<CreateIngredientRequest, IngredientDTO> {
  private _api = inject(IngredientApiService);

  async handle(request: CreateIngredientRequest): Promise<IngredientDTO> {
    return this._api.createIngredient({
      name: request.name,
      measurementUnit: request.measurementUnit
    });
  }
}

