import { Ingredient } from '../../core/models/ingredient.model';
import { IngredientDTO } from '../dtos/ingredient.dto';

export class IngredientMapper {
  static fromDTO(dto: IngredientDTO): Ingredient {
    return {
      id: dto.id,
      name: dto.name,
      measurementUnit: dto.measurementUnit
    } satisfies Ingredient;
  }
  
  static fromDTOs(dtos: IngredientDTO[]): Ingredient[] {
    return dtos.map(dto => IngredientMapper.fromDTO(dto));
  }
}
