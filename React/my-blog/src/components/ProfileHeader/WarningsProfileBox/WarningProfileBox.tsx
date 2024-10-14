import { Box, Typography } from "@mui/material"
import WarningIcon from '@mui/icons-material/Warning';
import DoneIcon from '@mui/icons-material/Done';
import CancelRoundedIcon from '@mui/icons-material/CancelRounded';
import { WarningProfileProps } from "./WarningProfileProps";
import "../WarningsProfileBox/WarningsProfileBox.scss";

const WarningProfileBox = ({ warnings, isBanned }: WarningProfileProps) => {
    return (
        <>
            {
                isBanned ?
                    <Box bgcolor="#BC7C7C" className="profile-header-warnings-info-container">
                        <Typography>User is blocked!</Typography>
                        <CancelRoundedIcon />
                    </Box>
                    :
                    warnings.length === 0 ?
                        <Box bgcolor="#00FF9C" className="profile-header-warnings-info-container">
                            <Typography>No active warns!</Typography>
                            <DoneIcon />
                        </Box>
                        :
                        <Box bgcolor="#FFE700" className="profile-header-warnings-info-container">
                            <Typography>{warnings.length}/3 active warns</Typography>
                            <WarningIcon />
                        </Box>
            }
        </>
    )
}

export { WarningProfileBox }