const express = require("express");
const path = require("path");

const app = express();
const PORT = process.env.PORT || 3000;

app.use(express.json());
app.use(express.static(path.join(__dirname, "public")));

const panelState = {
  brandName: "BATTLE MOBILE INDIA",
  matchConfig: {
    maxPlayers: 100,
    countdownSeconds: 25,
    selectedRegion: {
      level: "India",
      state: "Rajasthan",
      district: "Jaipur",
      sceneName: "03_Map_India_Main"
    }
  },
  regions: [
    {
      state: "Rajasthan",
      stateScene: "04_Map_State_Preset",
      districts: [
        { name: "Jaipur", districtScene: "05_Map_District_Preset" },
        { name: "Jodhpur", districtScene: "05_Map_District_Preset" }
      ]
    },
    {
      state: "Maharashtra",
      stateScene: "04_Map_State_Preset",
      districts: [
        { name: "Mumbai", districtScene: "05_Map_District_Preset" },
        { name: "Pune", districtScene: "05_Map_District_Preset" }
      ]
    }
  ],
  players: [
    { id: "BMI_10001", name: "BMI_Player_1", status: "Active", region: "India" }
  ]
};

app.get("/api/health", (req, res) => {
  res.json({ ok: true, brand: panelState.brandName, port: PORT });
});

app.get("/api/state", (req, res) => {
  res.json(panelState);
});

app.post("/api/auth/login", (req, res) => {
  const { playerName, playerId } = req.body || {};
  const cleanName = (playerName || "BMI_Guest").trim();
  const cleanId = (playerId || `BMI_${Math.floor(10000 + Math.random() * 90000)}`).trim();

  const existing = panelState.players.find((p) => p.id === cleanId);
  if (!existing) {
    panelState.players.push({
      id: cleanId,
      name: cleanName,
      status: "InLobby",
      region: panelState.matchConfig.selectedRegion.level
    });
  } else {
    existing.name = cleanName;
    existing.status = "InLobby";
  }

  res.json({
    ok: true,
    player: { id: cleanId, name: cleanName, status: "InLobby" },
    matchConfig: panelState.matchConfig
  });
});

app.post("/api/match/config", (req, res) => {
  const { maxPlayers, countdownSeconds, level, state, district, sceneName } = req.body || {};

  if (typeof maxPlayers === "number" && maxPlayers > 0) {
    panelState.matchConfig.maxPlayers = Math.min(100, Math.max(1, maxPlayers));
  }
  if (typeof countdownSeconds === "number" && countdownSeconds >= 0) {
    panelState.matchConfig.countdownSeconds = countdownSeconds;
  }

  panelState.matchConfig.selectedRegion = {
    level: level || panelState.matchConfig.selectedRegion.level,
    state: state || panelState.matchConfig.selectedRegion.state,
    district: district || panelState.matchConfig.selectedRegion.district,
    sceneName: sceneName || panelState.matchConfig.selectedRegion.sceneName
  };

  res.json({ ok: true, matchConfig: panelState.matchConfig });
});

app.listen(PORT, () => {
  console.log(`${panelState.brandName} Panel running on http://localhost:${PORT}`);
});

