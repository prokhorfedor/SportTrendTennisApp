import { useState } from "react";
import Login from "./Login";
import {
    Box,
    Tabs,
    Tab,
    Card,
    CardContent,
    Button,
    Typography,
    Alert,
} from "@mui/material";

// маленький хелпер для вкладок
function TabPanel(props) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            {...other}
        >
            {value === index && (
                <Box sx={{ mt: 2 }}>
                    {children}
                </Box>
            )}
        </div>
    );
}

export default function TennisRegistrationMUI() {
    const schedule = {
        Понеділок: [
            { id: 1, time: "10:00 - 11:00", level: "Початківці" },
            { id: 2, time: "18:00 - 19:00", level: "Середній рівень" },
        ],
        Середа: [
            { id: 3, time: "09:00 - 10:00", level: "Діти" },
            { id: 4, time: "19:00 - 20:30", level: "Просунуті" },
        ],
        Пʼятниця: [
            { id: 5, time: "17:00 - 18:00", level: "Змішана група" },
        ],
    };

    const [tabIndex, setTabIndex] = useState(0);
    const [selectedGroup, setSelectedGroup] = useState(null);
    const [loggedIn, setLoggedIn] = useState(false);

    const handleChange = (event, newValue) => {
        setTabIndex(newValue);
    };

    const handleRegister = (group) => {
        setSelectedGroup(group);
    };

    const days = Object.keys(schedule);

    return (
        <Box sx={{ maxWidth: 600, mx: "auto", p: 3 }}>
            {!loggedIn ? (
                <Login onLogin={() => setLoggedIn(true)} />
            ) : (
                <>
                    <Typography variant="h4" align="center" gutterBottom>
                        Реєстрація на групу з тенісу
                    </Typography>
                    {/* Tabs */}
                    <Tabs
                        value={tabIndex}
                        onChange={handleChange}
                        centered
                        aria-label="тенісні групи"
                    >
                        {days.map((day, idx) => (
                            <Tab key={day} label={day} />
                        ))}
                    </Tabs>
                    {/* TabsContent */}
                    {days.map((day, idx) => (
                        <TabPanel key={day} value={tabIndex} index={idx}>
                            {schedule[day].map((group) => (
                                <Card key={group.id} sx={{ mb: 2, borderRadius: 2, boxShadow: 2 }}>
                                    <CardContent
                                        sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                        }}
                                    >
                                        <Box>
                                            <Typography variant="subtitle1">{group.time}</Typography>
                                            <Typography variant="body2" color="text.secondary">
                                                {group.level}
                                            </Typography>
                                        </Box>
                                        <Button
                                            variant="contained"
                                            onClick={() => handleRegister(group)}
                                        >
                                            Записатись
                                        </Button>
                                    </CardContent>
                                </Card>
                            ))}
                        </TabPanel>
                    ))}
                    {/* Alert при виборі */}
                    {selectedGroup && (
                        <Alert severity="success" sx={{ mt: 3 }}>
                            ✅ Ви зареєструвались: {selectedGroup.level} ({selectedGroup.time})
                        </Alert>
                    )}
                </>
            )}
        </Box>
    );
}
