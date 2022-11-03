import {Box, Button, Checkbox, Chip, Input, InputLabel, MenuItem, Paper, Select, Typography} from '@mui/material';
import React, {useState} from 'react';
import {Filter, FilterLogicalOperator} from '../../shared/api/types/paging';
import {FilterMenuProps} from './FilterMenuProps';

const FilterMenu = ({availableFilters, width, requestFilters, setFilters}: FilterMenuProps) => {


    const [filterDropdown, setFilterDropdown] = useState<string>("");
    const [filterValue, setFilterValue] = useState<string>("");
    const [intersectFilters, setIntersectFilters] = useState<boolean>(requestFilters.logicalOperator === FilterLogicalOperator.And);

    const handleChangeLogicalOperator = () => {
        setFilters({
            ...requestFilters,
            logicalOperator: intersectFilters ? FilterLogicalOperator.And : FilterLogicalOperator.Or
        })
    }

    const handleDeleteFilter = (triggeredFilter: Filter) => {
        setFilters({
            logicalOperator: intersectFilters ? FilterLogicalOperator.And : FilterLogicalOperator.Or,
            filters: requestFilters.filters.filter((filter) => filter.path !== triggeredFilter.path || filter.value !== triggeredFilter.value)
        });
    }


    const handleAddFilter = () => {
        if (filterDropdown && filterValue) {
            setFilters({
                logicalOperator: intersectFilters ? FilterLogicalOperator.And : FilterLogicalOperator.Or,
                filters: requestFilters.filters.concat({path: filterDropdown, value: filterValue})
            });
        }
    }

    return (
        <Paper style={{minWidth:"500px",width: width, margin: "0 auto", minHeight: "50px", padding: "20px"}}>


            <Box style={{paddingBottom: "20px"}}>
                <Typography>Filtering menu:</Typography>
            </Box>

            <Box sx={{minWidth: "120px", display: "flex", justifyContent: "space-around"}}>

                <Box style={{maxWidth: "20%"}}>
                    <InputLabel id="filter-selector">Filter name</InputLabel>
                    <Select sx={{minWidth: "150px"}} value={filterDropdown}
                            onChange={(e) => setFilterDropdown(e.target.value)}>
                        {availableFilters.map((value, index) => <MenuItem value={value} key={index}>{value}</MenuItem>)}
                    </Select>
                </Box>

                <Box>
                    <InputLabel id="filter-value-input">Filter value</InputLabel>
                    <Input value={filterValue} onChange={(e) => {
                        setFilterValue(e.target.value)
                    }} type="text" name="filter-value"/>
                </Box>

                <Box>
                    <InputLabel id="filter-intersection">Combine filters</InputLabel>
                    <Checkbox value={intersectFilters} onChange={() => {
                        setIntersectFilters(!intersectFilters);
                        handleChangeLogicalOperator()
                    }}/>
                </Box>

                <Box>
                    <Button disabled={filterDropdown.length === 0 || filterValue.length === 0} variant="outlined"
                            onClick={handleAddFilter}>Add filter</Button>
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