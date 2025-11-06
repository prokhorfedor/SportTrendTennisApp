import { Box, Button, Typography } from "@mui/material";

export interface ShowSignUpComponentProps {
    goToSignup: () => void;
    compact?: boolean; // if true render a compact inline button
}
export function ShowSignUpComponent(props: ShowSignUpComponentProps) {
    if (props.compact) {
        return (
            <Button variant="outlined" color="primary" onClick={props.goToSignup} sx={{ height: '100%' }}>
                Sign up
            </Button>
        );
    }
    return (
        <Box sx={{ mt: 2 }}>
            <Typography color="warning.main" variant="body2" gutterBottom>
                User not found. Would you like to create a new account?
            </Typography>
            <Button variant="outlined" color="primary" fullWidth onClick={props.goToSignup}>Create Account</Button>
        </Box>
    );
}