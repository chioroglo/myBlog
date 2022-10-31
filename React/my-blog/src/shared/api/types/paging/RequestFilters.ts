import {Filter} from "./Filter";
import {FilterLogicalOperator} from "./FilterLogicalOperator";

export interface RequestFilters {
    logicalOperator: FilterLogicalOperator,
    filters: Filter[]
}