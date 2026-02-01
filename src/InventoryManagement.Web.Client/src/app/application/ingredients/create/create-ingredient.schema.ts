import { z } from 'zod';
import { MEASUREMENT_UNIT_OPTIONS } from '@core/models/measurement-unit.model';

export const createIngredientSchema = z.object({
  name: z.string().min(3, { message: "validation.min_length" }),
  measurementUnit: z.enum(MEASUREMENT_UNIT_OPTIONS as [string, ...string[]], {
      message: "validation.invalid_enum"
  })
});

export type CreateIngredientSchema = z.infer<typeof createIngredientSchema>;
