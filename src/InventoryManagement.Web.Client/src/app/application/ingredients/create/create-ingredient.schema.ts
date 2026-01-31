import { z } from 'zod';
import { MEASUREMENT_UNIT_OPTIONS } from '@core/models/measurement-unit.model';

export const createIngredientSchema = z.object({
  name: z.string().min(3, { message: "Name must be at least 3 characters long" }),
  measurementUnit: z.enum(MEASUREMENT_UNIT_OPTIONS as [string, ...string[]], {
      message: "Invalid measurement unit"
  })
});

export type CreateIngredientSchema = z.infer<typeof createIngredientSchema>;
