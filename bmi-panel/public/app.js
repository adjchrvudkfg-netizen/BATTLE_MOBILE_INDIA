const maxPlayersInput = document.getElementById("maxPlayersInput");
const countdownInput = document.getElementById("countdownInput");
const levelSelect = document.getElementById("levelSelect");
const stateSelect = document.getElementById("stateSelect");
const districtSelect = document.getElementById("districtSelect");
const saveConfigBtn = document.getElementById("saveConfigBtn");
const stateOutput = document.getElementById("stateOutput");

let regionsData = [];

function populateDistricts() {
  const selectedState = regionsData.find((r) => r.state === stateSelect.value) || regionsData[0];
  districtSelect.innerHTML = "";
  if (!selectedState) return;

  for (const d of selectedState.districts) {
    const opt = document.createElement("option");
    opt.value = d.name;
    opt.textContent = d.name;
    districtSelect.appendChild(opt);
  }
}

function populateStates(currentRegion) {
  stateSelect.innerHTML = "";
  for (const r of regionsData) {
    const opt = document.createElement("option");
    opt.value = r.state;
    opt.textContent = r.state;
    stateSelect.appendChild(opt);
  }
  if (currentRegion && currentRegion.state) {
    stateSelect.value = currentRegion.state;
  }
  populateDistricts();
  if (currentRegion && currentRegion.district) {
    districtSelect.value = currentRegion.district;
  }
}

async function loadState() {
  const res = await fetch("/api/state");
  const data = await res.json();
  regionsData = data.regions || [];

  maxPlayersInput.value = data.matchConfig.maxPlayers;
  countdownInput.value = data.matchConfig.countdownSeconds;
  levelSelect.value = data.matchConfig.selectedRegion.level;
  populateStates(data.matchConfig.selectedRegion);

  stateOutput.textContent = JSON.stringify(data, null, 2);
}

stateSelect.addEventListener("change", populateDistricts);

saveConfigBtn.addEventListener("click", async () => {
  const level = levelSelect.value;
  const state = stateSelect.value;
  const district = districtSelect.value;
  const sceneName =
    level === "State"
      ? "04_Map_State_Preset"
      : level === "District"
      ? "05_Map_District_Preset"
      : "03_Map_India_Main";

  const payload = {
    maxPlayers: Number(maxPlayersInput.value),
    countdownSeconds: Number(countdownInput.value),
    level,
    state,
    district,
    sceneName
  };

  await fetch("/api/match/config", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });

  await loadState();
});

loadState();

