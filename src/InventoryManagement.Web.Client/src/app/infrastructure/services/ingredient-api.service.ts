import { Injectable, signal } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BaseApiService } from './base-api.service';
import { Ingredient } from '../../core/models/ingredient.model';
import { CreateIngredientRequest, IngredientDTO } from '../dtos/ingredient.dto';
import { IngredientMapper } from '../mappers/ingredient.mapper';

@Injectable({
  providedIn: 'root'
})
export class IngredientApiService extends BaseApiService {
  readonly ingredients = signal<Ingredient[]>([]);
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  getIngredients(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.get<IngredientDTO[]>('/api/ingredients')
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: (dtos) => {
          const models = IngredientMapper.fromDTOs(dtos);
          this.ingredients.set(models);
        },
        error: (err) => {
          this.error.set(err.message);
        }
      });
  }

  createIngredient(request: CreateIngredientRequest): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.post<CreateIngredientRequest, IngredientDTO>('/api/ingredients', request)
      .pipe(
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: (newDto) => {
          const newModel = IngredientMapper.fromDTO(newDto);
          this.ingredients.update((current) => [...current, newModel]);
        },
        error: (err) => {
          this.error.set(err.message);
        }
      });
  }
}
