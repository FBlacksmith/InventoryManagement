export type MeasurementUnitName = 'Grams' | 'Liters' | 'Units' | 'Kilograms' | 'Milliliters';

export const MEASUREMENT_UNITS: Record<string, MeasurementUnitName> = {
  Grams: 'Grams',
  Liters: 'Liters',
  Units: 'Units',
  Kilograms: 'Kilograms',
  Milliliters: 'Milliliters'
} as const;

export const MEASUREMENT_UNIT_OPTIONS = Object.values(MEASUREMENT_UNITS);
