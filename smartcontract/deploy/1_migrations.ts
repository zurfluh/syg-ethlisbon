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
    await galaxyToken.transferOwnership(spaceMafia.address, {gasLimit: 12450000});

    await spaceMafia.initialize(galaxyToken.address, {gasLimit: 12450000});

    const planetType = await spaceMafia.planetType();
    console.log('planetType: ', planetType.toString());

    // MINT PLANETS
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'Lobstrum', {gasLimit: 12450000})
    await spaceMafia.mintPlanet(accounts.deployer,'Crayons', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'Squiddy', {gasLimit: 12450000})
    await spaceMafia.mintPlanet(accounts.deployer,'Whaylu', {gasLimit: 12450000})
    await spaceMafia.mintPlanet("0xbf652059f7fE27e4e39dBD3B5B5E5eAbC34c6741",'Sharky', {gasLimit: 12450000})
    console.log('Mint successsful')
    await spaceMafia.transferPlanet("0x9b39b00989854A89B4f70Df0D2a432C68d9c1306", planetType.add('2').toString(), {gasLimit: 12450000});
    console.log('Transfer successful:', await spaceMafia.getPlanet(planetType.add('1').toString()));
  };
  module.exports.tags = ['All'];