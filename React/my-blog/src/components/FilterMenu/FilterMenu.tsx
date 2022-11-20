import {
    Box,
    Button,
    Chip,
    FormControlLabel,
    Input,
    InputLabel,
    MenuItem,
    Paper,
    Select,
    Switch,
    Typography
} from '@mui/material';
import React, {useState} from 'react';
import {Filter, FilterLogicalOperator} from '../../shared/api/types/paging';
import {FilterMenuProps} from './FilterMenuProps';
import {useSearchParams} from "react-router-dom";
import {requestFiltersToBrowserQueryString} from "../../shared/assets/requestFiltersToBrowserQueryString";
import SearchIcon from '@mui/icons-material/Search';

const FilterMenu = ({availableFilters, width, requestFilters, setFilters}: FilterMenuProps) => {

    const [searchParams, setSearchParams] = useSearchParams();

    const [doCombineFiltersWithOrOperator, setDoCombineFiltersWithOrOperator] = useState<boolean>(requestFilters.logicalOperator === FilterLogicalOperator.Or);
    const [filterPathDropdown, setFilterPathDropdown] = useState<string>("");
    const [filterValue, setFilterValue] = useState<string>("");


    const alreadyHasNoSuchFilter = (filter: Filter) => {
        return !requestFilters.filters.some((val) => val.path === filter.path && val.value === filter.value);
    }

    const handleDeleteFilter = (triggeredFilter: Filter) => {


        let removedTriggeredFilter = requestFilters.filters.filter((filter) => (filter.path !== triggeredFilter.path) || (filter.value !== triggeredFilter.value));

        setSearchParams(requestFiltersToBrowserQueryString({
            filters: removedTriggeredFilter,
            logicalOperator: requestFilters.logicalOperator
        }));

        setFilters({
            logicalOperator: requestFilters.logicalOperator,
            filters: removedTriggeredFilter
        });
    }

    const handleAddFilter = () => {
        const newFilters = requestFilters.filters.concat({path: filterPathDropdown, value: filterValue});


        setSearchParams(requestFiltersToBrowserQueryString({
            filters: newFilters,
            logicalOperator: requestFilters.logicalOperator
        }));

        if (filterPathDropdown && filterValue && alreadyHasNoSuchFilter({
            path: filterPathDropdown,
            value: filterValue
        })) {
            setFilters({
                logicalOperator: requestFilters.logicalOperator,
                filters: newFilters
            });
        }

    }

    const handleChangeOfLogicalOperator = () => {

        const newLogicalOperator = doCombineFiltersWithOrOperator ? FilterLogicalOperator.And : FilterLogicalOperator.Or;

        setSearchParams(requestFiltersToBrowserQueryString({
            filters: requestFilters.filters,
            logicalOperator: newLogicalOperator
        }));

        if (doCombineFiltersWithOrOperator) {
            setFilters({...requestFilters, logicalOperator: FilterLogicalOperator.And});
        } else {
            setFilters({...requestFilters, logicalOperator: FilterLogicalOperator.Or});
        }

        setDoCombineFiltersWithOrOperator(!doCombineFiltersWithOrOperator);
    }


    return (
        <Paper elevation={12}
               style={{minWidth: "500px", width: width, margin: "0 auto", minHeight: "50px", padding: "20px"}}>


            <Box style={{paddingBottom: "20px"}}>
                <Typography style={{fontSize: "36px"}}>
                    <SearchIcon fontSize={"large"}/>
                    Search
                </Typography>
            </Box>

            <Box sx={{padding: "20px", minWidth: "120px", display: "flex", justifyContent: "space-around"}}>

                <Box style={{maxWidth: "20%"}}>
                    <InputLabel id="filter-selector">Filter name</InputLabel>
                    <Select sx={{minWidth: "150px"}} value={filterPathDropdown}
                            onChange={(e) => setFilterPathDropdown(e.target.value)}>
                        {availableFilters.map((value, index) => <MenuItem value={value} key={index}>{value}</MenuItem>)}
                    </Select>
                </Box>

                <Box>
                    <InputLabel id="filter-value-input">Filter value</InputLabel>
                    <Input placeholder={"Search..."} value={filterValue} onChange={(e) => {
                        setFilterValue(e.target.value)
                    }} type="text" name="filter-value"/>
                </Box>

                <Box>
                    <Button disabled={filterPathDropdown.length === 0 || filterValue.length === 0} variant="outlined"
                            onClick={handleAddFilter}>Add filter</Button>
                </Box>

                <Box style={{minWidth: "150px"}}>
                    {
                        requestFilters.filters.length > 1
                            ?
                            <>
                                <InputLabel id="filter-intersection">Search separately</InputLabel>
                                <FormControlLabel style={{margin: 0, display: "flex", justifyContent: "center"}}
                                                  control={<Switch checked={doCombineFiltersWithOrOperator}
                                                                   onChange={handleChangeOfLogicalOperator}/>}
                                                  label=""/>
                            </>
                            :
                            <></>
                    }
                </Box>

            </Box>
            <>
                {requestFilters?.filters.map((filter, index) =>
                    <Chip color="info" key={index} onDelete={() => handleDeleteFilter(filter)} label={
                        <>
                            <span style={{fontWeight: "bold"}}>{filter.path}</span> : {filter.value}
                        </>
                    }/>)}
            </>
        </Paper>
    );
};

export {FilterMenu};