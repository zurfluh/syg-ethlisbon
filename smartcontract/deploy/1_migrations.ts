const hre = require("hardhat");
module.exports = async({getNamedAccounts, deployments}) => {
    const accounts = await getNamedAccounts()
    const GalaxyToken = await hre.ethers.getContractFactory("GalaxyToken");
    const galaxyToken = await GalaxyToken.deploy({gasLimit: 12450000});
    console.log("galaxyToken deployed to: ", galaxyToken.address);
    const SpaceMafia = await hre.ethers.getContractFactory("SpaceMafia");
    const spaceMafia = await SpaceMafia.deploy({gasLimit: 12450000});
    await spaceMafia.deployed();
    console.log("spaceMafia deployed to:", spaceMafia.address);
    await galaxyToken.setApprovalForAll(spaceMafia.address, true, {gasLimit: 12450000});
    await galaxyToken.transferOwnership(spaceMafia.address, {gasLimit: 12450000});

    await spaceMafia.initialize(galaxyToken.address, {gasLimit: 12450000});

    const planetType = await spaceMafia.planetType();
    console.log('planetType: ', planetType);

    // MINT PLANETS
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'', {gasLimit: 12450000})
  };
  module.exports.tags = ['All'];