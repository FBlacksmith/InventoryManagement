import { MeasurementUnitName } from './measurement-unit.model';

export interface Ingredient {
  id: string;
  name: string;
  measurementUnit: MeasurementUnitName;
}
